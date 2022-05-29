using Abp.Auditing;
using BTIT.EPM.Configuration.Dto;

namespace BTIT.EPM.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}