using Abp.AspNetCore.Mvc.Views;

namespace BTIT.EPM.Web.Views
{
    public abstract class EPMRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected EPMRazorPage()
        {
            LocalizationSourceName = EPMConsts.LocalizationSourceName;
        }
    }
}
