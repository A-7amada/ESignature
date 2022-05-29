using System.Threading.Tasks;
using Abp.Application.Services;
using BTIT.EPM.Editions.Dto;
using BTIT.EPM.MultiTenancy.Dto;

namespace BTIT.EPM.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}