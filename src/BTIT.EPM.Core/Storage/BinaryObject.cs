using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities;

namespace BTIT.EPM.Storage
{
    [Table("AppBinaryObjects")]
    public class BinaryObject : Entity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public virtual string Description { get; set; }

        [Required] public virtual byte[] Bytes { get; set; }

        public string ContentType { get; set; }

        public string FileExtension { get; set; }
        public string FileName { get; set; }

        public BinaryObject()
        {
            Id = SequentialGuidGenerator.Instance.Create();
        }

        public BinaryObject(int? tenantId, byte[] bytes, string description = null)
: this()
        {
            TenantId = tenantId;
            Bytes = bytes;
            Description = description;
        }


        public BinaryObject(int? tenantId, byte[] bytes, string contentType, string description)
    : this()
        {
            TenantId = tenantId;
            Bytes = bytes;
            Description = description;
            ContentType = contentType;
        }

        public BinaryObject(int? tenantId, byte[] bytes, string contentType, string fileExtension, string description)
            : this()
        {
            TenantId = tenantId;
            Bytes = bytes;
            Description = description;
            ContentType = contentType;
            FileExtension = fileExtension;
        }
        public BinaryObject(int? tenantId, byte[] bytes, string contentType, string fileExtension, string description, string fileName)
            : this()
        {
            TenantId = tenantId;
            Bytes = bytes;
            Description = description;
            ContentType = contentType;
            FileExtension = fileExtension;
            FileName = fileName;
        }
    }
}
