using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}