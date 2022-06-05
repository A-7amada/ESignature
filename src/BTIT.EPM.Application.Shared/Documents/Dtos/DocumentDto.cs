using System;
using Abp.Application.Services.Dto;
using BTIT.EPM.Lookups;

namespace BTIT.EPM.Documents.Dtos
{
    public class DocumentDto : EntityDto<long>
    {
        public string FileName { get; set; }

        public string Extension { get; set; }

        public long? Size { get; set; }

        public string ContentType { get; set; }

        public string Comment { get; set; }

        public int DocumentTypeEnum { get; set; }

        public long? DocumentBagId { get; set; }

        public Guid? BinaryObjectId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public byte[] BinaryObjectBytes { get; set; }

    }
}