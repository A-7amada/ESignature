using BTIT.EPM.Editions;
using BTIT.EPM.Editions.Dto;
using BTIT.EPM.MultiTenancy.Payments;
using BTIT.EPM.Security;
using BTIT.EPM.MultiTenancy.Payments.Dto;

namespace BTIT.EPM.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
