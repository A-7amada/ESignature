using System.Collections.Generic;
using System.Linq;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using BTIT.EPM.Authorization.Permissions;
using BTIT.EPM.Authorization.Permissions.Dto;
using BTIT.EPM.Web.Areas.App.Models.Common.Modals;
using BTIT.EPM.Web.Controllers;
using System.Threading.Tasks;
using BTIT.EPM.Storage;
using System;
using System.Net;
using BTIT.EPM.DigitalSignature;
using BTIT.EPM.Lookups;
using Microsoft.AspNetCore.Authorization;
using Abp.Domain.Uow;

namespace BTIT.EPM.Web.Areas.App.Controllers
{
    [Area("App")]
    public class CommonController : EPMControllerBase
    {
        private readonly IPermissionAppService _permissionAppService;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IRecipientsAppService _recipientsAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;



        public CommonController(IPermissionAppService permissionAppService,
                                IBinaryObjectManager binaryObjectManager,
                                IRecipientsAppService recipientsAppService,
                                IUnitOfWorkManager unitOfWorkManager)
        {
            _permissionAppService = permissionAppService;
            _binaryObjectManager = binaryObjectManager;
            _recipientsAppService = recipientsAppService;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [AbpMvcAuthorize]
        public PartialViewResult LookupModal(LookupModalViewModel model)
        {
            return PartialView("Modals/_LookupModal", model);
        }

        [AbpMvcAuthorize]
        public PartialViewResult EntityTypeHistoryModal(EntityHistoryModalViewModel input)
        {
            return PartialView("Modals/_EntityTypeHistoryModal", ObjectMapper.Map<EntityHistoryModalViewModel>(input));
        }

        [AbpMvcAuthorize]
        public PartialViewResult PermissionTreeModal(List<string> grantedPermissionNames = null)
        {
            var permissions = _permissionAppService.GetAllPermissions().Items.ToList();

            var model = new PermissionTreeModalViewModel
            {
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissionNames
            };

            return PartialView("Modals/_PermissionTreeModal", model);
        }

        [AbpMvcAuthorize]
        public PartialViewResult InactivityControllerNotifyModal()
        {
            return PartialView("Modals/_InactivityControllerNotifyModal");
        }


        [AllowAnonymous]
        public async Task<ActionResult> GetFile(Guid id, string contentType, long? recipientId = null)
        {
            var tenantId = AbpSession.TenantId;
            if (!tenantId.HasValue)
                tenantId = 1;

            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                if (recipientId != null)
                {
                    await _recipientsAppService.RecipientViewDocument(recipientId.Value);

                }

                var fileObject = await _binaryObjectManager.GetOrNullAsync(id);
                if (fileObject == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }

                return File(fileObject.Bytes, contentType);
            }
        }
    }
}