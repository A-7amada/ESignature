using Abp.AutoMapper;
using BTIT.EPM.Sessions.Dto;

namespace BTIT.EPM.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}