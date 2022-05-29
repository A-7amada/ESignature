using System.Threading.Tasks;
using Abp.Application.Services;
using BTIT.EPM.MultiTenancy.Payments.Dto;
using BTIT.EPM.MultiTenancy.Payments.Stripe.Dto;

namespace BTIT.EPM.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}