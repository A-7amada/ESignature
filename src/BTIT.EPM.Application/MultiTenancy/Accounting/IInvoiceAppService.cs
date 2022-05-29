using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using BTIT.EPM.MultiTenancy.Accounting.Dto;

namespace BTIT.EPM.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
