using Abp.Application.Services.Dto;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}