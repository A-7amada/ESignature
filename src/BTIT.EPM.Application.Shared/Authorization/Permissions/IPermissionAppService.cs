using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BTIT.EPM.Authorization.Permissions.Dto;

namespace BTIT.EPM.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
