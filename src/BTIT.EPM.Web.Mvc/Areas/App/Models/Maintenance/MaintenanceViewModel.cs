using System.Collections.Generic;
using BTIT.EPM.Caching.Dto;

namespace BTIT.EPM.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}