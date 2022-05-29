using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Dto;
using BTIT.EPM.Lookups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTIT.EPM.DigitalSignature
{
	public interface IDocumentRequestAuditTrailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDocumentRequestAuditTrailForViewDto>> GetAll(GetAllDocumentRequestAuditTrailsInput input);
		Task<DocumentAuditTrailDto> GetDocumentRequestAuditTrailForView(long documentRequestId, long documentId);
		Task<GetDocumentRequestAuditTrailForEditOutput> GetDocumentRequestAuditTrailForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditDocumentRequestAuditTrailDto input);

		Task AddAuditByDocumentRequestIdAsync(long documentRequestId, AuditTrailType type, long? recipientId = null);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetDocumentRequestAuditTrailsToExcel(GetAllDocumentRequestAuditTrailsForExcelInput input);

		
		Task<List<DocumentRequestAuditTrailDocumentRequestLookupTableDto>> GetAllDocumentRequestForTableDropdown();
		
		Task<List<DocumentRequestAuditTrailRecipientLookupTableDto>> GetAllRecipientForTableDropdown();
		
    }
}