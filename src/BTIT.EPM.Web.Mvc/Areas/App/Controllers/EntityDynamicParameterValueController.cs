using System;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.DynamicEntityParameters;
using Abp.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using BTIT.EPM.Authorization;
using BTIT.EPM.Web.Areas.App.Models.EntityDynamicParameterValues;
using BTIT.EPM.Web.Controllers;

namespace BTIT.EPM.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_EntityDynamicParameterValue)]
    public class EntityDynamicParameterValueController : EPMControllerBase
    {
        private readonly IDynamicEntityParameterDefinitionManager _dynamicEntityParameterDefinitionManager;

        public EntityDynamicParameterValueController(IDynamicEntityParameterDefinitionManager dynamicEntityParameterDefinitionManager)
        {
            _dynamicEntityParameterDefinitionManager = dynamicEntityParameterDefinitionManager;
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Edit)]
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Create)]
        [HttpGet("/App/EntityDynamicParameterValue/ManageAll/{entityFullName}/{entityId}")]
        public IActionResult ManageAll(string entityFullName, string entityId)
        {
            if (entityFullName.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(entityFullName));
            }

            if (entityId.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            if (!_dynamicEntityParameterDefinitionManager.ContainsEntity(entityFullName))
            {
                throw new UserFriendlyException(L("UnknownEntityType", entityFullName));
            }

            return View(new EntityDynamicParameterValueManageAllViewModel
            {
                EntityFullName = entityFullName,
                EntityId = entityId
            });
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Edit)]
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Create)]
        public IActionResult ManageEntityDynamicParameterValuesModal(string entityFullName, string rowId)
        {
            if (entityFullName.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(entityFullName));
            }

            if (rowId.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(rowId));
            }

            if (!_dynamicEntityParameterDefinitionManager.ContainsEntity(entityFullName))
            {
                throw new UserFriendlyException(L("UnknownEntityType", entityFullName));
            }

            return PartialView("_ManageEntityDynamicParameterValuesModal", new EntityDynamicParameterValueManageAllViewModel
            {
                EntityFullName = entityFullName,
                EntityId = rowId
            });
        }
    }
}