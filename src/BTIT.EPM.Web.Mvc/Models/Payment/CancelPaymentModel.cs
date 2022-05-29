using BTIT.EPM.MultiTenancy.Payments;

namespace BTIT.EPM.Web.Models.Payment
{
    public class CancelPaymentModel
    {
        public string PaymentId { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }
    }
}