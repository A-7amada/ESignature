﻿using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace BTIT.EPM.Web.Public.Views
{
    public abstract class EPMRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected EPMRazorPage()
        {
            LocalizationSourceName = EPMConsts.LocalizationSourceName;
        }
    }
}
