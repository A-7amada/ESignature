using System.Collections.Generic;
using Abp.Application.Services.Dto;
using BTIT.EPM.Configuration.Host.Dto;
using BTIT.EPM.Editions.Dto;

namespace BTIT.EPM.Web.Areas.App.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}