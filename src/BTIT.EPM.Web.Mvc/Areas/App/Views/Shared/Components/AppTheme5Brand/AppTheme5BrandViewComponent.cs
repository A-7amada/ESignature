﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BTIT.EPM.Web.Areas.App.Models.Layout;
using BTIT.EPM.Web.Session;
using BTIT.EPM.Web.Views;

namespace BTIT.EPM.Web.Areas.App.Views.Shared.Components.AppTheme5Brand
{
    public class AppTheme5BrandViewComponent : EPMViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme5BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
