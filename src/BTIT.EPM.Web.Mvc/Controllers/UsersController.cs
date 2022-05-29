using Abp.AspNetCore.Mvc.Authorization;
using BTIT.EPM.Authorization;
using BTIT.EPM.Storage;
using Abp.BackgroundJobs;
using Abp.Authorization;

namespace BTIT.EPM.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}