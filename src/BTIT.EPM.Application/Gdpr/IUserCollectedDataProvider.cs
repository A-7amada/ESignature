using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using BTIT.EPM.Dto;

namespace BTIT.EPM.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
