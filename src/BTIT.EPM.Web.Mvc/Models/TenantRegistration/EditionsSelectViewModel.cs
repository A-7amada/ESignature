using Abp.AutoMapper;
using BTIT.EPM.MultiTenancy.Dto;

namespace BTIT.EPM.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
