using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}