using System.Threading.Tasks;
using BTIT.EPM.Sessions.Dto;

namespace BTIT.EPM.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
