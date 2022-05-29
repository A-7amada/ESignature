using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using BTIT.EPM.Web.Areas.App.Models.Documents;
using BTIT.EPM.Web.Controllers;
using BTIT.EPM.Authorization;
using BTIT.EPM.Documents;
using BTIT.EPM.Documents.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using System.Net;

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

        public ActionResult Index()
        {
            var model = new DocumentsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Documents_Create, AppPermissions.Pages_Documents_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetDocumentForEditOutput getDocumentForEditOutput;

			if (id.HasValue){
				getDocumentForEditOutput = await _documentsAppService.GetDocumentForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getDocumentForEditOutput = new GetDocumentForEditOutput{
					Document = new CreateOrEditDocumentDto()
				};
			}

            var viewModel = new CreateOrEditDocumentModalViewModel()
            {
				Document = getDocumentForEditOutput.Document,
					BinaryObjectTenantId = getDocumentForEditOutput.BinaryObjectTenantId,
					DocumentRequestDocumentTitle = getDocumentForEditOutput.DocumentRequestDocumentTitle,
					DocumentBinaryObjectList = await _documentsAppService.GetAllBinaryObjectForTableDropdown(),
					DocumentDocumentRequestList = await _documentsAppService.GetAllDocumentRequestForTableDropdown(),                
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewDocumentModal(long id)
        {
			var getDocumentForViewDto = await _documentsAppService.GetDocumentForView(id);

            var model = new DocumentViewModel()
            {
                Document = getDocumentForViewDto.Document
                , BinaryObjectTenantId = getDocumentForViewDto.BinaryObjectTenantId 

                , DocumentRequestDocumentTitle = getDocumentForViewDto.DocumentRequestDocumentTitle 

            };

            return PartialView("_ViewDocumentModal", model);
        }


        public async Task<ActionResult> DownloadFile(long documentId)
        {
            var file = await _documentsAppService.GetDocument(documentId);
            if (file == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            // to download file directly instead of Show it in browser
            var contentDispositionHeader = new System.Net.Mime.ContentDisposition 
            {
                Inline = false,
                FileName = file.FileName
            };
            Response.Headers.Add("Content-Disposition", contentDispositionHeader.ToString());

            return File(file.FileBytes, file.ContentType);
        }

    }
}