using System.Threading.Tasks;

namespace BTIT.EPM.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}