using Abp.Application.Services.Dto;

namespace BTIT.EPM.Documents.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}