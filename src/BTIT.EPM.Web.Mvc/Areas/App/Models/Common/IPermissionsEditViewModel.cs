using System.Collections.Generic;
using BTIT.EPM.Authorization.Permissions.Dto;

namespace BTIT.EPM.Web.Areas.App.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}