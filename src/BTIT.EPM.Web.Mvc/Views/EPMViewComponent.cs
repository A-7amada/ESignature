using Abp.AspNetCore.Mvc.ViewComponents;

namespace BTIT.EPM.Web.Views
{
    public abstract class EPMViewComponent : AbpViewComponent
    {
        protected EPMViewComponent()
        {
            LocalizationSourceName = EPMConsts.LocalizationSourceName;
        }
    }
}