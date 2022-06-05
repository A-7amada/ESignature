using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using BTIT.EPM.Web.Areas.App.Models.FileSignatures;
using BTIT.EPM.Web.Controllers;
using BTIT.EPM.Authorization;
using BTIT.EPM.ESignatureDemo;
using BTIT.EPM.ESignatureDemo.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using BTIT.EPM.Documents;
using Abp.UI;
using Abp.IO.Extensions;
using BTIT.EPM.Storage;
using Abp.Web.Models;
using System.IO;

namespace BTIT.EPM.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_FileSignatures)]
    public class FileSignaturesController : EPMControllerBase
    {
        private readonly IFileSignaturesAppService _fileSignaturesAppService;
        private readonly IDocumentsAppService _documentsAppService;
        private readonly IBinaryObjectManager _binaryObjectManager;
        public FileSignaturesController(IFileSignaturesAppService fileSignaturesAppService
            , IBinaryObjectManager binaryObjectManager
            , IDocumentsAppService documentsAppService)
        {
            _fileSignaturesAppService = fileSignaturesAppService;
            _documentsAppService = documentsAppService;
            _binaryObjectManager = binaryObjectManager;
        }

        public ActionResult Index()
        {
            var model = new FileSignaturesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FileSignatures_Create, AppPermissions.Pages_FileSignatures_Edit)]
        public async Task<ActionResult> CreateOrEdit(int? id)
        {
            GetFileSignatureForEditOutput getFileSignatureForEditOutput;

            if (id.HasValue)
            {
                getFileSignatureForEditOutput = await _fileSignaturesAppService.GetFileSignatureForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getFileSignatureForEditOutput = new GetFileSignatureForEditOutput
                {
                    FileSignature = new CreateOrEditFileSignatureDto()

                };
                getFileSignatureForEditOutput.FileSignature.DocumentBagId= _documentsAppService.GenerateDocmantBagId();
            }

            var viewModel = new CreateOrEditFileSignatureModalViewModel()
            {
                FileSignature = getFileSignatureForEditOutput.FileSignature,

            };

            return View("CreateOrEdit", viewModel);
        }

        public async Task<PartialViewResult> ViewFileSignatureModal(int id)
        {
            var getFileSignatureForViewDto = await _fileSignaturesAppService.GetFileSignatureForView(id);

            var model = new FileSignatureViewModel()
            {
                FileSignature = getFileSignatureForViewDto.FileSignature
            };

            return PartialView("_ViewFileSignatureModal", model);
        }
        [HttpPost]
        public async Task<JsonResult> UploadFile(string documentDescription = "")
        {
            try
            {
                var file = Request.Form.Files[0];

                //Check input
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 10485760) //10MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var fileObject = new BinaryObject(AbpSession.TenantId, fileBytes);
                await _binaryObjectManager.SaveAsync(fileObject);
                return Json(new AjaxResponse(new
                {
                    binaryObjectId = fileObject.Id,
                    contentType = file.ContentType,
                    fileName = string.IsNullOrEmpty(documentDescription) ? file.FileName : documentDescription,
                    extension = Path.GetExtension(file.FileName),
                    size = file.Length,
                    serviceRequestCommentId = 0
                }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

    }
}