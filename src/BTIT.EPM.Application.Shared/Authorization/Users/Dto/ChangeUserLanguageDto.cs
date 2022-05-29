using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
