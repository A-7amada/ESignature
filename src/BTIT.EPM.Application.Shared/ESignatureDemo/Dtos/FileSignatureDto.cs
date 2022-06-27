using System;
using Abp.Application.Services.Dto;

namespace BTIT.EPM.ESignatureDemo.Dtos
{
    public class FileSignatureDto : EntityDto
    {
        public string Name { get; set; }

        public string Describtion { get; set; }
        public string FileUrl { get; set; }

    }
}