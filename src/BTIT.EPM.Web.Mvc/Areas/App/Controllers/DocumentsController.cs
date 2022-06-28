using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using BTIT.EPM.Storage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BTIT.EPM.Web.Controllers;
using NUglify.Helpers;
using System.Collections.Generic;
using BTIT.EPM.Documents;
using System.IO;

//using BTIT.EPM.Web.Areas.App.Models.Documents;
using Abp.AspNetCore.Mvc.Authorization;
using BTIT.EPM.Authorization;
using BTIT.EPM.Documents.Dtos;
using BTIT.EPM.ESignatureDemo;
using Spire.Pdf;
using Spire.Pdf.Security;
using BTIT.EPM.Web.Areas.App.Models.Documents;

namespace BTIT.EPM.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Documents)]
    public class DocumentsController : EPMControllerBase
    {
        private readonly IDocumentsAppService _documentsAppService;

        public DocumentsController(IDocumentsAppService documentsAppService)
        {
            _documentsAppService = documentsAppService;
        }

   //     public ActionResult Index()
   //     {
   //         var model = new DocumentsViewModel
			//{
			//	FilterText = ""
			//};

   //         return View(model);
   //     } 

   //     [AbpMvcAuthorize(AppPermissions.Pages_Documents_Create, AppPermissions.Pages_Documents_Edit)]
   //     public async Task<PartialViewResult> CreateOrEditModal(long? id)
   //     {
			//GetDocumentForEditOutput getDocumentForEditOutput;

			//if (id.HasValue){
			//	getDocumentForEditOutput = await _documentsAppService.GetDocumentForEdit(new EntityDto<long> { Id = (long) id });
			//}
			//else {
			//	getDocumentForEditOutput = new GetDocumentForEditOutput{
			//		Document = new CreateOrEditDocumentDto()
			//	};
			//}

   //         var viewModel = new CreateOrEditDocumentModalViewModel()
   //         {
			//	Document = getDocumentForEditOutput.Document,
			//		BinaryObjectTenantId = getDocumentForEditOutput.BinaryObjectTenantId,
			//		DocumentRequestDocumentTitle = getDocumentForEditOutput.DocumentRequestDocumentTitle,
			//		DocumentBinaryObjectList = await _documentsAppService.GetAllBinaryObjectForTableDropdown(),
			//		DocumentDocumentRequestList = await _documentsAppService.GetAllDocumentRequestForTableDropdown(),                
   //         };

   //         return PartialView("_CreateOrEditModal", viewModel);
   //     }

   //     public async Task<PartialViewResult> ViewDocumentModal(long id)
   //     {
			//var getDocumentForViewDto = await _documentsAppService.GetDocumentForView(id);

   //         var model = new DocumentViewModel()
   //         {
   //             Document = getDocumentForViewDto.Document
   //             , BinaryObjectTenantId = getDocumentForViewDto.BinaryObjectTenantId 

   //             , DocumentRequestDocumentTitle = getDocumentForViewDto.DocumentRequestDocumentTitle 

   //         };

   //         return PartialView("_ViewDocumentModal", model);
   //     }


        //public async Task<ActionResult> DownloadFile(long documentId)
        //{
        //    var file = await _documentsAppService.GetDocument(documentId);
        //    if (file == null)
        //    {
        //        return StatusCode((int)HttpStatusCode.NotFound);
        //    }

        //    // to download file directly instead of Show it in browser
        //    var contentDispositionHeader = new System.Net.Mime.ContentDisposition 
        //    {
        //        Inline = false,
        //        FileName = file.FileName
        //    };
        //    Response.Headers.Add("Content-Disposition", contentDispositionHeader.ToString());

        //    return File(file.FileBytes, file.ContentType);
        //}

        [HttpPost]
        public async Task<JsonResult> UploadFile(long documentBagId, DocumentTypeEnum? documentTypeEnum = null, bool isCreateDocumentBagId = false, string documentDescription = "")
        {

            try
            {
                documentTypeEnum = DocumentTypeEnum.Pdf;

                var files = Request.Form.Files;
                var documentsDtoList = new List<CreateDocumentInputDto>();

                //Check input
                if (files == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (documentBagId == null || documentBagId == 0)
                {
                    documentBagId = _documentsAppService.GenerateDocmantBagId();
                }
                if (isCreateDocumentBagId && documentBagId == 0)
                {
                    documentBagId = _documentsAppService.GenerateDocmantBagId();
                }
                files.ForEach(fe =>
                {
                    var documentDto = new CreateDocumentInputDto();
                    if (fe.Length > 10485760) //10MB
                    {
                        throw new UserFriendlyException(L("File_SizeLimit_Error"));
                    }

                    documentDto.DocumentBagId = documentBagId;
                    // documentDto.DocumentTypeEnum = documentTypeEnum;
                    documentDto.FileName = fe.FileName;
                    documentDto.ContentType = fe.ContentType;
                    using (var stream = fe.OpenReadStream())
                    {
                        documentDto.FileBytes = stream.GetAllBytes();
                    }
                    documentDto.Description = documentDescription;
                    documentDto.Size = fe.Length;
                    documentDto.Extension = Path.GetExtension(fe.FileName);

                    documentsDtoList.Add(documentDto);

                
                });

                var documentId = await _documentsAppService.CreateDocuments(documentsDtoList);
                #region Pdf
                //var fe = documentsDtoList.FirstOrDefault();
                //PdfDocument doc = new PdfDocument(fe.FileBytes);
                
                //PdfPageBase page = doc.Pages[0];

                //PdfCertificate cert = new PdfCertificate("Demo.pfx", "e-iceblue");

                //PdfSignature signature = new PdfSignature(doc, page, cert, "demo");

                //signature.ContactInfo = "Harry";
                //signature.Certificated = true;

                //signature.DocumentPermissions = PdfCertificationFlags.AllowFormFill;

                //doc.SaveToFile("Result.pdf");

                #endregion

                return Json(new AjaxResponse(new
                {
                    id = documentId,
                    contentType = files.FirstOrDefault().ContentType,
                    documentDescription = string.IsNullOrEmpty(documentDescription) ? files.FirstOrDefault().FileName : documentDescription
                }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        public async Task<ActionResult> GetFileBydocumentBagId(long documentBagId)
        {
            var documents = _documentsAppService.GetDocuments(documentBagId);

            return View(documents);
        }

        public async Task<ActionResult> GetFile(long documentId)
        {
            var fileObject = await _documentsAppService.GetDocumentForView(documentId);
            if (fileObject == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            return File(fileObject.Document.BinaryObjectBytes, fileObject.Document.ContentType);
        }

        public async Task<PartialViewResult> ViewFileModal(string fileUrl)
        {
            var model = new DocumentViewModel()
            {
               FileUrl= fileUrl
            };

            return PartialView("_ViewFileModal", model);
        }

    }
}