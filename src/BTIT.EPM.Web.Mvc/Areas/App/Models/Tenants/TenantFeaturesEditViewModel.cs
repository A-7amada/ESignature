using Abp.AutoMapper;
using BTIT.EPM.MultiTenancy;
using BTIT.EPM.MultiTenancy.Dto;
using BTIT.EPM.Web.Areas.App.Models.Common;

namespace BTIT.EPM.Web.Areas.App.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}