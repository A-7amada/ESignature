using System.Collections.Generic;
using Abp.Application.Services.Dto;
using BTIT.EPM.Authorization.Permissions.Dto;
using BTIT.EPM.Web.Areas.App.Models.Common;

namespace BTIT.EPM.Web.Areas.App.Models.Roles
{
    public class RoleListViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}