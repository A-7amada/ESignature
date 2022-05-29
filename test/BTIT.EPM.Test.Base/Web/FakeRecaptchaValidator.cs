using System.Threading.Tasks;
using BTIT.EPM.Security.Recaptcha;

namespace BTIT.EPM.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
