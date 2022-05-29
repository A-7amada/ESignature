using System.Threading.Tasks;
using Abp.Application.Services;
using BTIT.EPM.MultiTenancy.Payments.PayPal.Dto;

namespace BTIT.EPM.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
