using Microsoft.AspNetCore.Mvc;
using BTIT.EPM.Web.Controllers;

namespace BTIT.EPM.Web.Public.Controllers
{
    public class HomeController : EPMControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}