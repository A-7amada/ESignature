using System.Collections.Generic;
using BTIT.EPM.DynamicEntityParameters.Dto;

namespace BTIT.EPM.Web.Areas.App.Models.DynamicParameters
{
    public class CreateOrEditDynamicParameterViewModel
    {
        public DynamicParameterDto DynamicParameterDto { get; set; }

        public List<string> AllowedInputTypes { get; set; }
    }
}
