using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace BTIT.EPM.Web.Controllers
{
    public class HomeController : EPMControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Ui");
        }
    }
}
