using Abp.AspNetCore.Mvc.Controllers;
using Abp.Auditing;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using BTIT.EPM.EntityFrameworkCore;
using BTIT.EPM.Install;
using BTIT.EPM.Migrations.Seed.Host;
using BTIT.EPM.Web.Models.Install;
using Newtonsoft.Json.Linq;

namespace BTIT.EPM.Web.Controllers
{
    [DisableAuditing]
    public class InstallController : AbpController
    {
        private readonly IInstallAppService _installAppService;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly DatabaseCheckHelper _databaseCheckHelper;

        public InstallController(
            IInstallAppService installAppService, 
            IHostApplicationLifetime applicationLifetime, 
            DatabaseCheckHelper databaseCheckHelper)
        {
            _installAppService = installAppService;
            _applicationLifetime = applicationLifetime;
            _databaseCheckHelper = databaseCheckHelper;
        }

        [UnitOfWork(IsDisabled = true)]
        public ActionResult Index()
        {
            var appSettings = _installAppService.GetAppSettingsJson();
            var connectionString = GetConnectionString();

            if (_databaseCheckHelper.Exist(connectionString))
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new InstallViewModel
            {
                Languages = DefaultLanguagesCreator.InitialLanguages,
                AppSettingsJson = appSettings
            };
            
            return View(model);
        }

        public ActionResult Restart()
        {
            _applicationLifetime.StopApplication();
            return View();
        }

        private string GetConnectionString()
        {
            var appsettingsjson = JObject.Parse(System.IO.File.ReadAllText("appsettings.json"));
            var connectionStrings = (JObject)appsettingsjson["ConnectionStrings"];
            return connectionStrings.Property(EPMConsts.ConnectionStringName).Value.ToString();
        }
    }
}