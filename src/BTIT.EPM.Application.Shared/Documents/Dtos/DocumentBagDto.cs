using System;
using Abp.Application.Services.Dto;

namespace BTIT.EPM.Documents.Dtos
{
    public class DocumentBagDto : EntityDto<long>
    {
        public string DocumentBagId { get; set; }

    }
}