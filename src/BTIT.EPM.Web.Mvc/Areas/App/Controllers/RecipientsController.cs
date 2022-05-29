using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using BTIT.EPM.Web.Areas.App.Models.Recipients;
using BTIT.EPM.Web.Controllers;
using BTIT.EPM.Authorization;
using BTIT.EPM.DigitalSignature;
using BTIT.EPM.DigitalSignature.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace BTIT.EPM.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Recipients)]
    public class RecipientsController : EPMControllerBase
    {
        private readonly IRecipientsAppService _recipientsAppService;

        public RecipientsController(IRecipientsAppService recipientsAppService)
        {
            _recipientsAppService = recipientsAppService;
        }

        public ActionResult Index()
        {
            var model = new RecipientsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Recipients_Create, AppPermissions.Pages_Recipients_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetRecipientForEditOutput getRecipientForEditOutput;

			if (id.HasValue){
				getRecipientForEditOutput = await _recipientsAppService.GetRecipientForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getRecipientForEditOutput = new GetRecipientForEditOutput{
					Recipient = new CreateOrEditRecipientDto()
				};
				getRecipientForEditOutput.Recipient.ViewDate = DateTime.Now;
				getRecipientForEditOutput.Recipient.SignatureDate = DateTime.Now;
				getRecipientForEditOutput.Recipient.SignerPinExpiry = DateTime.Now;
				getRecipientForEditOutput.Recipient.SentDate = DateTime.Now;
			}

            var viewModel = new CreateOrEditRecipientModalViewModel()
            {
				Recipient = getRecipientForEditOutput.Recipient,
					UserName = getRecipientForEditOutput.UserName,
					DocumentRequestDocumentTitle = getRecipientForEditOutput.DocumentRequestDocumentTitle,
					RecipientUserList = await _recipientsAppService.GetAllUserForTableDropdown(),
					RecipientDocumentRequestList = await _recipientsAppService.GetAllDocumentRequestForTableDropdown(),                
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRecipientModal(long id)
        {
			var getRecipientForViewDto = await _recipientsAppService.GetRecipientForView(id);

            var model = new RecipientViewModel()
            {
                Recipient = getRecipientForViewDto.Recipient
                , UserName = getRecipientForViewDto.UserName 

                , DocumentRequestDocumentTitle = getRecipientForViewDto.DocumentRequestDocumentTitle 

            };

            return PartialView("_ViewRecipientModal", model);
        }


    }
}