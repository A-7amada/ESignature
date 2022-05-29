using System.Threading.Tasks;
using Abp.Dependency;

namespace BTIT.EPM.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}