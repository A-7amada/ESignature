using System.Collections.Generic;
using Abp.Application.Services.Dto;
using BTIT.EPM.Editions.Dto;

namespace BTIT.EPM.Web.Areas.App.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}