using System;
using Abp.Application.Services.Dto;
using BTIT.EPM.Lookups;

namespace BTIT.EPM.Documents.Dtos
{
    public class DocumentDto : EntityDto<long>
    {
        public byte[] FileBytes { set; get; }

        public string FileName { get; set; }

        public string Extension { get; set; }

        public int Size { get; set; }

        public string ContentType { get; set; }

        public bool IsActive { get; set; }

        public Guid? BinaryObjectId { get; set; }

        public long? DocumentRequestId { get; set; }

        public string CreatedDate { get; set; }
        public string Recipients { get; set; }
        public DocumentRequestStatus Status { get; set; }

    }
}