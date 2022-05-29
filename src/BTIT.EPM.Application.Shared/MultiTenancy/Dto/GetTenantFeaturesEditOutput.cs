using System.Collections.Generic;
using Abp.Application.Services.Dto;
using BTIT.EPM.Editions.Dto;

namespace BTIT.EPM.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}