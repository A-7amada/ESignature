using BTIT.EPM.Dto;

namespace BTIT.EPM.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
