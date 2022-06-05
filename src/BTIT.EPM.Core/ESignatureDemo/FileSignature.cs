using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace BTIT.EPM.ESignatureDemo
{
    [Table("FileSignatures")]
    public class FileSignature : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Describtion { get; set; }

    }
}