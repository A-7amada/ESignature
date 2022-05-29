using Abp.AspNetCore.Mvc.ViewComponents;

namespace BTIT.EPM.Web.Public.Views
{
    public abstract class EPMViewComponent : AbpViewComponent
    {
        protected EPMViewComponent()
        {
            LocalizationSourceName = EPMConsts.LocalizationSourceName;
        }
    }
}