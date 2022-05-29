using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BTIT.EPM.Documents.Dtos;
using BTIT.EPM.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BTIT.EPM.Documents
{
	public interface IDocumentsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDocumentForViewDto>> GetAll(GetAllDocumentsInput input);
		Task<PagedResultDto<GetDocumentForViewDto>> GetUserDocuments(GetAllDocumentsInput input);

		Task<GetDocumentForViewDto> GetDocumentForView(long id);

		Task<GetDocumentForEditOutput> GetDocumentForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditDocumentDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetDocumentsToExcel(GetAllDocumentsForExcelInput input);

		
		Task<List<DocumentBinaryObjectLookupTableDto>> GetAllBinaryObjectForTableDropdown();
		
		Task<List<DocumentDocumentRequestLookupTableDto>> GetAllDocumentRequestForTableDropdown();
		Task<DocumentDto> GetDocument(long documentId);
	}
}