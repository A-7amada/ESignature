﻿using System.Collections.Generic;
using BTIT.EPM.DashboardCustomization.Dto;

namespace BTIT.EPM.Web.Areas.App.Models.CustomizableDashboard
{
    public class AddWidgetViewModel
    {
        public List<WidgetOutput> Widgets { get; set; }

        public string DashboardName { get; set; }

        public string PageId { get; set; }
    }
}
