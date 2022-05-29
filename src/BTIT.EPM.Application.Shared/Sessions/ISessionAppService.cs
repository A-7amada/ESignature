using System.Threading.Tasks;
using Abp.Application.Services;
using BTIT.EPM.Sessions.Dto;

namespace BTIT.EPM.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
