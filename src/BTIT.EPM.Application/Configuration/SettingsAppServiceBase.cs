using System.Threading.Tasks;
using Abp.Net.Mail;
using BTIT.EPM.Configuration.Host.Dto;

namespace BTIT.EPM.Configuration
{
    public abstract class SettingsAppServiceBase : EPMAppServiceBase
    {
        private readonly IEmailSender _emailSender;

        protected SettingsAppServiceBase(
            IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        #region Send Test Email

        public async Task SendTestEmail(SendTestEmailInput input)
        {
            await _emailSender.SendAsync(
                input.EmailAddress,
                L("TestEmail_Subject"),
                L("TestEmail_Body")
            );
        }

        #endregion
    }
}
