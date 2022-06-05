using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.ESignatureDemo.Dtos
{
    public class CreateOrEditFileSignatureDto : EntityDto<int?>
    {

        public string Name { get; set; }

        public string Describtion { get; set; }
        public long? DocumentBagId { get; set; }

    }
}