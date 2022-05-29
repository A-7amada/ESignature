﻿using Abp.Application.Services.Dto;

namespace BTIT.EPM.DynamicEntityParameters.Dto
{
    public class EntityDynamicParameterValueDto : EntityDto
    {
        public string Value { get; set; }

        public string EntityId { get; set; }

        public int EntityDynamicParameterId { get; set; }

        public int? TenantId { get; set; }
    }
}
