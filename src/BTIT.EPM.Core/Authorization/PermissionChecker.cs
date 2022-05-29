using Abp.Authorization;
using BTIT.EPM.Authorization.Roles;
using BTIT.EPM.Authorization.Users;

namespace BTIT.EPM.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
