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

        Task<GetDocumentForViewDto> GetDocumentForView(long id);

        Task<List<GetDocumentForViewDto>> GetDocuments(long? documentBagId = null, long? documentTypeEnum = null);

        Task<GetDocumentForEditOutput> GetDocumentForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateDocumentInputDto input);

        Task<long> CreateDocuments(List<CreateDocumentInputDto> filesDto);

        Task Delete(long id);

        Task<FileDto> GetDocumentsToExcel(GetAllDocumentsForExcelInput input);

        Task<List<DocumentDocumentBagLookupTableDto>> GetAllDocumentBagForTableDropdown();

        Task<List<DocumentBinaryObjectLookupTableDto>> GetAllBinaryObjectForTableDropdown();

        long GenerateDocmantBagId();
    }
}