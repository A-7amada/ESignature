using System.Threading.Tasks;

namespace BTIT.EPM.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}