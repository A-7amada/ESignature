using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using BTIT.EPM.Web.Areas.App.Models.Contacts;
using BTIT.EPM.Web.Controllers;
using BTIT.EPM.Authorization;
using BTIT.EPM.DigitalSignature;
using BTIT.EPM.DigitalSignature.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace BTIT.EPM.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Contacts)]
    public class ContactsController : EPMControllerBase
    {
        private readonly IContactsAppService _contactsAppService;

        public ContactsController(IContactsAppService contactsAppService)
        {
            _contactsAppService = contactsAppService;
        }

        public ActionResult Index()
        {
            var model = new ContactsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Contacts_Create, AppPermissions.Pages_Contacts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetContactForEditOutput getContactForEditOutput;

			if (id.HasValue){
				getContactForEditOutput = await _contactsAppService.GetContactForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getContactForEditOutput = new GetContactForEditOutput{
					Contact = new CreateOrEditContactDto()
				};
			}

            var viewModel = new CreateOrEditContactModalViewModel()
            {
				Contact = getContactForEditOutput.Contact,                
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewContactModal(long id)
        {
			var getContactForViewDto = await _contactsAppService.GetContactForView(id);

            var model = new ContactViewModel()
            {
                Contact = getContactForViewDto.Contact
            };

            return PartialView("_ViewContactModal", model);
        }


    }
}