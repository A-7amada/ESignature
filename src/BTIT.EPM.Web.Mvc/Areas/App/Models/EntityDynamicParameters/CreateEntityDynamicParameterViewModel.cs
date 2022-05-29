using System.Collections.Generic;
using BTIT.EPM.DynamicEntityParameters.Dto;

namespace BTIT.EPM.Web.Areas.App.Models.EntityDynamicParameters
{
    public class CreateEntityDynamicParameterViewModel
    {
        public string EntityFullName { get; set; }

        public List<string> AllEntities { get; set; }

        public List<DynamicParameterDto> DynamicParameters { get; set; }
    }
}
