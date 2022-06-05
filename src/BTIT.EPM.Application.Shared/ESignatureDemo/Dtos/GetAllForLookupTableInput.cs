using Abp.Application.Services.Dto;

namespace BTIT.EPM.ESignatureDemo.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}