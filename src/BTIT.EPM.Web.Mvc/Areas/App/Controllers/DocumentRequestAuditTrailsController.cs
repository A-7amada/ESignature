using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using BTIT.EPM.Authorization;
using BTIT.EPM.DigitalSignature;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Web.Areas.App.Models.DocumentRequestAuditTrails;
using BTIT.EPM.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BTIT.EPM.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_DocumentRequestAuditTrails)]
    public class DocumentRequestAuditTrailsController : EPMControllerBase
    {
        private readonly IDocumentRequestAuditTrailsAppService _documentRequestAuditTrailsAppService;

        public DocumentRequestAuditTrailsController(IDocumentRequestAuditTrailsAppService documentRequestAuditTrailsAppService)
        {
            _documentRequestAuditTrailsAppService = documentRequestAuditTrailsAppService;
        }

        public ActionResult Index()
        {
            var model = new DocumentRequestAuditTrailsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DocumentRequestAuditTrails_Create, AppPermissions.Pages_DocumentRequestAuditTrails_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetDocumentRequestAuditTrailForEditOutput getDocumentRequestAuditTrailForEditOutput;

            if (id.HasValue)
            {
                getDocumentRequestAuditTrailForEditOutput = await _documentRequestAuditTrailsAppService.GetDocumentRequestAuditTrailForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getDocumentRequestAuditTrailForEditOutput = new GetDocumentRequestAuditTrailForEditOutput
                {
                    DocumentRequestAuditTrail = new CreateOrEditDocumentRequestAuditTrailDto()
                };
            }

            var viewModel = new CreateOrEditDocumentRequestAuditTrailModalViewModel()
            {
                DocumentRequestAuditTrail = getDocumentRequestAuditTrailForEditOutput.DocumentRequestAuditTrail,
                DocumentRequestDocumentTitle = getDocumentRequestAuditTrailForEditOutput.DocumentRequestDocumentTitle,
                RecipientFirstName = getDocumentRequestAuditTrailForEditOutput.RecipientFirstName,
                DocumentRequestAuditTrailDocumentRequestList = await _documentRequestAuditTrailsAppService.GetAllDocumentRequestForTableDropdown(),
                DocumentRequestAuditTrailRecipientList = await _documentRequestAuditTrailsAppService.GetAllRecipientForTableDropdown(),
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        //     public async Task<PartialViewResult> ViewDocumentRequestAuditTrailModal(long id)
        //     {
        //var getDocumentRequestAuditTrailForViewDto = await _documentRequestAuditTrailsAppService.GetDocumentRequestAuditTrailForView(id);

        //        var model = new DocumentRequestAuditTrailViewModel()
        //         {
        //             DocumentRequestAuditTrail = getDocumentRequestAuditTrailForViewDto.DocumentRequestAuditTrail
        //             , DocumentRequestDocumentTitle = getDocumentRequestAuditTrailForViewDto.DocumentRequestDocumentTitle 

        //             , RecipientFirstName = getDocumentRequestAuditTrailForViewDto.RecipientFirstName 

        //         };

        //         return PartialView("_ViewDocumentRequestAuditTrailModal", model);
        //     }


        public async Task<ActionResult> DocumentRequestAuditTrail(long documentRequestId, long documentId)
        {
            var model = await _documentRequestAuditTrailsAppService.GetDocumentRequestAuditTrailForView(documentRequestId, documentId);
            return View(model);
        }

    }
}