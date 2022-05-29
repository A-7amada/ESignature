using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace BTIT.EPM.Localization.Dto
{
    public class GetLanguageForEditOutput
    {
        public ApplicationLanguageEditDto Language { get; set; }

        public List<ComboboxItemDto> LanguageNames { get; set; }
        
        public List<ComboboxItemDto> Flags { get; set; }

        public GetLanguageForEditOutput()
        {
            LanguageNames = new List<ComboboxItemDto>();
            Flags = new List<ComboboxItemDto>();
        }
    }
}