using Abp.AspNetCore.Mvc.Authorization;
using BTIT.EPM.Storage;

namespace BTIT.EPM.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
        }
    }
}