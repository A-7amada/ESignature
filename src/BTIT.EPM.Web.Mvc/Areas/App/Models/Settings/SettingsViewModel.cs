using System.Collections.Generic;
using Abp.Application.Services.Dto;
using BTIT.EPM.Configuration.Tenants.Dto;

namespace BTIT.EPM.Web.Areas.App.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }
        
        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}