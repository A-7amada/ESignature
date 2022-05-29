using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}