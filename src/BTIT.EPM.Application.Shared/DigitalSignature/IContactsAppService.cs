using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Dto;


namespace BTIT.EPM.DigitalSignature
{
    public interface IContactsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetContactForViewDto>> GetAll(GetAllContactsInput input);

        Task<GetContactForViewDto> GetContactForView(long id);

		Task<GetContactForEditOutput> GetContactForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditContactDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetContactsToExcel(GetAllContactsForExcelInput input);

		
    }
}