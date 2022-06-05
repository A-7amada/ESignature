using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BTIT.EPM.ESignatureDemo.Dtos;
using BTIT.EPM.Dto;

namespace BTIT.EPM.ESignatureDemo
{
    public interface IFileSignaturesAppService : IApplicationService
    {
        Task<PagedResultDto<GetFileSignatureForViewDto>> GetAll(GetAllFileSignaturesInput input);

        Task<GetFileSignatureForViewDto> GetFileSignatureForView(int id);

        Task<GetFileSignatureForEditOutput> GetFileSignatureForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditFileSignatureDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetFileSignaturesToExcel(GetAllFileSignaturesForExcelInput input);

    }
}