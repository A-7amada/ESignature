using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Dto;
using System.Collections.Generic;
using System.Collections.Generic;


namespace BTIT.EPM.DigitalSignature
{
    public interface IRecipientsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRecipientForViewDto>> GetAll(GetAllRecipientsInput input);

        Task<GetRecipientForViewDto> GetRecipientForView(long id);

		Task<GetRecipientForEditOutput> GetRecipientForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditRecipientDto input);

		Task Delete(EntityDto<long> input);

		Task RecipientViewDocument(long id);

		Task<FileDto> GetRecipientsToExcel(GetAllRecipientsForExcelInput input);

		
		Task<List<RecipientUserLookupTableDto>> GetAllUserForTableDropdown();
		
		Task<List<RecipientDocumentRequestLookupTableDto>> GetAllDocumentRequestForTableDropdown();
		
    }
}