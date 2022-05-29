using System.Collections.Generic;
using BTIT.EPM.Editions;
using BTIT.EPM.Editions.Dto;
using BTIT.EPM.MultiTenancy.Payments;
using BTIT.EPM.MultiTenancy.Payments.Dto;

namespace BTIT.EPM.Web.Models.Payment
{
    public class BuyEditionViewModel
    {
        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}
