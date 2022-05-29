using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace BTIT.EPM.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
