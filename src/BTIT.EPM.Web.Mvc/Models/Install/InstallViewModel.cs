using System.Collections.Generic;
using Abp.Localization;
using BTIT.EPM.Install.Dto;

namespace BTIT.EPM.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
