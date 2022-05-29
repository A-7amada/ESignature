using Abp.AutoMapper;
using BTIT.EPM.MultiTenancy.Dto;

namespace BTIT.EPM.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}