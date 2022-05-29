﻿using Abp.AutoMapper;
using BTIT.EPM.Authorization.Roles.Dto;
using BTIT.EPM.Web.Areas.App.Models.Common;

namespace BTIT.EPM.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}