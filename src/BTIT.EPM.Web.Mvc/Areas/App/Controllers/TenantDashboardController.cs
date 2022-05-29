using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using BTIT.EPM.Authorization;
using BTIT.EPM.DashboardCustomization;
using BTIT.EPM.Web.DashboardCustomization;
using System.Threading.Tasks;

namespace BTIT.EPM.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardController : CustomizableDashboardControllerBase
    {
        public TenantDashboardController(DashboardViewConfiguration dashboardViewConfiguration, 
            IDashboardCustomizationAppService dashboardCustomizationAppService) 
            : base(dashboardViewConfiguration, dashboardCustomizationAppService)
        {

        }

        public async Task<ActionResult> Index()
        {
            return await GetView(EPMDashboardCustomizationConsts.DashboardNames.DefaultTenantDashboard);
        }
    }
}