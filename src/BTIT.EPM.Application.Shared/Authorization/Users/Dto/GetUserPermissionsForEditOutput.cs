using System.Collections.Generic;
using BTIT.EPM.Authorization.Permissions.Dto;

namespace BTIT.EPM.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}