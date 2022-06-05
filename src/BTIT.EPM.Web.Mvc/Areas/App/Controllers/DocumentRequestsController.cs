using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using BTIT.EPM.Authorization;
using BTIT.EPM.CustomExtension;
using BTIT.EPM.DigitalSignature;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Documents;
using BTIT.EPM.Documents.Dtos;
using BTIT.EPM.Identity;
using BTIT.EPM.Lookups;
using BTIT.EPM.Storage;
using BTIT.EPM.Url;
using BTIT.EPM.Web.Areas.App.Models.DocumentRequests;
using BTIT.EPM.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BTIT.EPM.Web.Areas.App.Controllers
{
    [Area("App")]    
    public class DocumentRequestsController : EPMControllerBase
    {
        private readonly IDocumentRequestsAppService _documentRequestsAppService;
        private readonly IWebUrlService _webUrlService;
        private readonly SignInManager _signInManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IDocumentsAppService _documentAppService;
        private readonly IDocumentRequestAuditTrailsAppService _documentRequestAuditTrailsAppService;

        public DocumentRequestsController(IDocumentRequestsAppService documentRequestsAppService,
                                          IBinaryObjectManager binaryObjectManager,
                                          IDocumentsAppService documentsAppService,
                                          IDocumentRequestAuditTrailsAppService documentRequestAuditTrailsAppService,
                                          SignInManager signInManager,
                                          IWebUrlService webUrlService)
        {
            _documentRequestsAppService = documentRequestsAppService;
            _binaryObjectManager = binaryObjectManager;
            _documentAppService = documentsAppService;
            _documentRequestAuditTrailsAppService = documentRequestAuditTrailsAppService;
        }

        public ActionResult Index()
        {
            var model = new DocumentRequestsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DocumentRequests_Create, AppPermissions.Pages_DocumentRequests_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            //GetDocumentRequestForEditOutput getDocumentRequestForEditOutput;

            //if (id.HasValue){
            //	getDocumentRequestForEditOutput = await _documentRequestsAppService.GetDocumentRequestForEdit(new EntityDto<long> { Id = (long) id });
            //}
            //else {
            //	getDocumentRequestForEditOutput = new GetDocumentRequestForEditOutput{
            //		DocumentRequest = new CreateOrEditDocumentRequestDto()
            //	};
            //}

            var viewModel = new CreateOrEditDocumentRequestModalViewModel()
            {
                //DocumentRequest = getDocumentRequestForEditOutput.DocumentRequest,                
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }



        [AbpMvcAuthorize(AppPermissions.Pages_DocumentRequests_Create, AppPermissions.Pages_DocumentRequests_Edit)]
        public async Task<ActionResult> Create(int? id)
        {
            CreateOrEditAllRecipientsDto getDocumentRequestForEditOutput;

            if (id.HasValue)
            {
                getDocumentRequestForEditOutput = await _documentRequestsAppService.GetDocumentRequestForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getDocumentRequestForEditOutput = new CreateOrEditAllRecipientsDto
                {
                    Recipients = new List<CreateOrEditRecipientDto>()
                };
            }

            var viewModel = new CreateOrEditDocumentRequestModalViewModel
            {
                DocumentRequestId = getDocumentRequestForEditOutput.DocumentRequestId,
                DocumentTitle = getDocumentRequestForEditOutput.DocumentTitle,
                BinaryObjectId = getDocumentRequestForEditOutput.BinaryObjectId,
                BinaryObjectContentType = getDocumentRequestForEditOutput.BinaryObjectContentType,
                IsSigningOrdered = getDocumentRequestForEditOutput.IsSigningOrdered,
                MessageBody = getDocumentRequestForEditOutput.MessageBody,
                Recipients = getDocumentRequestForEditOutput.Recipients
            };

            return View(viewModel);
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_DocumentRequests)]
        public async Task<ActionResult> UploadFileAndSaveDocumentRequest(string defaultFileUploadTextInput)
        {
            try
            {
                var file = Request.Form.Files.First();
                List<PdfLoadedSignatureField> DocumentSignatureFields = new List<PdfLoadedSignatureField>();
                var signaturesNames = new List<string>();

                ValidateFile(file);

                var fileBytes = GetFileBytesAndSignatures(signaturesNames, DocumentSignatureFields, file);

                var fileObject = new BinaryObject(AbpSession.TenantId, fileBytes);

                //================== Save Binary and Get ID =============================
                var objectBinaryId = await _binaryObjectManager.SaveAndGetIdAsync(fileObject);

                var documentRequest = new CreateOrEditDocumentRequestDto
                {
                    Status = DocumentRequestStatus.Draft
                };

                //================== Save Document Request and Get ID =============================
                var documentRequestId = await _documentRequestsAppService.CreateOrEdit(documentRequest);

                var document = new CreateOrEditDocumentDto
                {
                    BinaryObjectId = objectBinaryId,
                    ContentType = file.ContentType,
                    Extension = file.FileName.Split('.').LastOrDefault(),
                    FileName = file.FileName,
                    IsActive = true,
                    DocumentRequestId = documentRequestId,
                    Size = (int)file.Length
                    //TODO: waiting to convert fileSize to long
                };

                //================== Save Document =============================
                //await _documentAppService.CreateOrEdit(document);
                //await _documentRequestAuditTrailsAppService.AddAuditByDocumentRequestIdAsync(documentRequestId, AuditTrailType.Created);


                return Json(new AjaxResponse(new
                {
                    documentRequestId = documentRequestId,
                    signatures = signaturesNames,
                    id = fileObject.Id,
                    contentType = file.ContentType,
                    defaultFileUploadTextInput = string.IsNullOrEmpty(defaultFileUploadTextInput) ? file.FileName : defaultFileUploadTextInput
                }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DocumentRequests)]
        public async Task<PartialViewResult> ViewDocumentRequestModal(long id)
        {
            var getDocumentRequestForViewDto = await _documentRequestsAppService.GetDocumentRequestForView(id);

            var model = new DocumentRequestViewModel()
            {
                DocumentRequest = getDocumentRequestForViewDto.DocumentRequest
            };

            return PartialView("_ViewDocumentRequestModal", model);
        }

        private void ValidateFile(IFormFile file)
        {
            if (file == null)
            {
                throw new UserFriendlyException(L("File_Empty_Error"));
            }

            if (file.Length > 1048576) //1MB
            {
                throw new UserFriendlyException(L("File_SizeLimit_Error"));
            }

            if (file.FileName.Split('.').LastOrDefault().ToLower() != "pdf")
            {
                throw new UserFriendlyException(L("ExtensionFileNotAllowed"));
            }
        }


        private byte[] GetFileBytesAndSignatures(List<string> signaturesNames, List<PdfLoadedSignatureField> DocumentSignatureFields, IFormFile file)
        {
            byte[] fileBytes;
            using (var stream = file.OpenReadStream())
            {
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(stream);

                if (loadedDocument.Form != null)
                {
                    foreach (var item in loadedDocument.Form.Fields)
                    {
                        if (item is PdfLoadedSignatureField)
                        {
                            DocumentSignatureFields.Add(item as PdfLoadedSignatureField);
                        }
                    }
                }

                if (DocumentSignatureFields.Count == 0)
                {
                    throw new UserFriendlyException(L("No_SignatureFields_Found_Error"));
                }

                foreach (var signature in DocumentSignatureFields)
                {
                    signaturesNames.Add(signature.Name);
                }

                stream.Position = 0;

                fileBytes = stream.GetAllBytes();
                return fileBytes;
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DocumentRequests)]
        public async Task<ActionResult> ViewRequestAuditTrail(long id)
        {
            var getDocumentRequestAuditTrailsForViewDto = await _documentRequestsAppService.GetDocumentRequestAuditTrail(id);
            DocumentRequestAuditTrailsForViewModel input = new DocumentRequestAuditTrailsForViewModel()
            {
                DocumentId = getDocumentRequestAuditTrailsForViewDto.DocumentId,
                DocumentRequestAuditTrailForViewDetailsDto = getDocumentRequestAuditTrailsForViewDto.DocumentRequestAuditTrailForViewDetailsDto,
                FileName = getDocumentRequestAuditTrailsForViewDto.FileName,
                DocumentRequestDocumentTitle = getDocumentRequestAuditTrailsForViewDto.DocumentRequestDocumentTitle,
                Status = getDocumentRequestAuditTrailsForViewDto.Status
            };
            return View((DocumentRequestAuditTrailsForViewModel)input);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DocumentRequests)]
        public async Task<ActionResult> ViewAndSignDocument([FromQuery]ViewAndSignDocumentEmailViewModel input)
        {
            ViewAndSignDocumentViewModel _viewAndSignDocumentViewModel = new ViewAndSignDocumentViewModel();
            

            _viewAndSignDocumentViewModel.ViewAndSignDocumentEmailViewModel = input;
            var getDocumentRequestForViewDto = await _documentRequestsAppService.GetDocumentRequestForViewWithRecipients(input.DocumentRequestId);

            _viewAndSignDocumentViewModel.DocumentRequest = new DocumentRequestViewModel();
            _viewAndSignDocumentViewModel.DocumentRequest.DocumentRequest = getDocumentRequestForViewDto.DocumentRequest;
            _viewAndSignDocumentViewModel.Recipients = getDocumentRequestForViewDto.Recipients.ToList();

            var _selectedRecipient = _viewAndSignDocumentViewModel.Recipients.Where(r => r.Code == Guid.Parse(input.RecipientCode)).FirstOrDefault();
            _viewAndSignDocumentViewModel.ShowSignButton = _selectedRecipient.IsSigner && !_selectedRecipient.IsSigned;

            if(_selectedRecipient.UserId != AbpSession.UserId)
                throw new UserFriendlyException("This link has been sent to different user");

            var getDocumentRequestAuditTrailsForViewDto = await _documentRequestsAppService.GetDocumentRequestAuditTrail(input.DocumentRequestId);
            _viewAndSignDocumentViewModel.DocumentRequestAuditTrailsForViewModel = new DocumentRequestAuditTrailsForViewModel()
            {
                DocumentId = getDocumentRequestAuditTrailsForViewDto.DocumentId,
                DocumentRequestAuditTrailForViewDetailsDto = getDocumentRequestAuditTrailsForViewDto.DocumentRequestAuditTrailForViewDetailsDto,
                FileName = getDocumentRequestAuditTrailsForViewDto.FileName,
                DocumentRequestDocumentTitle = getDocumentRequestAuditTrailsForViewDto.DocumentRequestDocumentTitle,
                Status = getDocumentRequestAuditTrailsForViewDto.Status
            };

            return View((ViewAndSignDocumentViewModel)_viewAndSignDocumentViewModel);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ViewAndSignDocumentWithPin([FromQuery]ViewAndSignDocumentEmailViewModel input)
        {
            await _documentRequestsAppService.GeneratePin(input);
            return View(input);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ViewAndSignDocumentConfirmedPin([FromQuery]ViewAndSignDocumentEmailViewModel input)
        {
            ViewAndSignDocumentViewModel _viewAndSignDocumentViewModel = new ViewAndSignDocumentViewModel();

            var getDocumentRequestForViewDto = await _documentRequestsAppService.GetDocumentRequestForViewWithRecipients(input.DocumentRequestId);
            _viewAndSignDocumentViewModel.DocumentRequest = new DocumentRequestViewModel();
            _viewAndSignDocumentViewModel.DocumentRequest.DocumentRequest = getDocumentRequestForViewDto.DocumentRequest;
            _viewAndSignDocumentViewModel.Recipients = getDocumentRequestForViewDto.Recipients.ToList();
            var _selectedRecipient = _viewAndSignDocumentViewModel.Recipients.Where(r => r.Code == Guid.Parse(input.RecipientCode)).FirstOrDefault();
            _viewAndSignDocumentViewModel.ShowSignButton = _selectedRecipient.IsSigner && !_selectedRecipient.IsSigned;

            if(_selectedRecipient.SignerPin!= input.SignerPin)
            {
                if (!_documentRequestsAppService.CheckPin(_selectedRecipient.Id, input.SignerPin))
                return View("ViewAndSignDocumentWithPin", input);
            }

            var getDocumentRequestAuditTrailsForViewDto = await _documentRequestsAppService.GetDocumentRequestAuditTrail(input.DocumentRequestId);
            _viewAndSignDocumentViewModel.DocumentRequestAuditTrailsForViewModel = new DocumentRequestAuditTrailsForViewModel()
            {
                DocumentId = getDocumentRequestAuditTrailsForViewDto.DocumentId,
                DocumentRequestAuditTrailForViewDetailsDto = getDocumentRequestAuditTrailsForViewDto.DocumentRequestAuditTrailForViewDetailsDto,
                FileName = getDocumentRequestAuditTrailsForViewDto.FileName,
                DocumentRequestDocumentTitle = getDocumentRequestAuditTrailsForViewDto.DocumentRequestDocumentTitle,
                Status = getDocumentRequestAuditTrailsForViewDto.Status
            };

            _viewAndSignDocumentViewModel.ViewAndSignDocumentEmailViewModel = input;

            return View("ViewAndSignDocument", (ViewAndSignDocumentViewModel)_viewAndSignDocumentViewModel);
        }

        [AllowAnonymous]
        public async Task<PartialViewResult> DigitalSignature()
        {
            return PartialView("_DigitalSignature");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> OnPostSignature([FromBody]PostSignature signatureData)
        {
            NameValueCollection qscoll = HttpUtility.ParseQueryString(signatureData.c);

            var queryString = signatureData.c.Substring(2).Split('&');

            var viewAndSignDocumentEmailViewModel = new ViewAndSignDocumentEmailViewModel();
            viewAndSignDocumentEmailViewModel.c = qscoll["c"];
            viewAndSignDocumentEmailViewModel.ManualResolveParameters();
            if (String.IsNullOrWhiteSpace(signatureData.SignatureDataUrl)) return Json("EmpatyLink");

            var base64Signature = signatureData.SignatureDataUrl.Split(",")[1];
            var binarySignature = Convert.FromBase64String(base64Signature);

            if(!_documentRequestsAppService.CheckPin(viewAndSignDocumentEmailViewModel.RecipientId, qscoll["SignerPin"]))
                throw new UserFriendlyException(L("IcorrectPin"));

            await _documentRequestsAppService.UpdateDocument(viewAndSignDocumentEmailViewModel.RecipientId, viewAndSignDocumentEmailViewModel.RecipientCode, 
                binarySignature, qscoll["SignerPin"]);
           
            
            return Json("done");
        }

    }
}