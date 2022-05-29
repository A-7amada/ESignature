using BTIT.EPM.Dto;

namespace BTIT.EPM.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}