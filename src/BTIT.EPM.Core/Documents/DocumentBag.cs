using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace BTIT.EPM.Documents
{
    [Table("DocumentBags")]
    public class DocumentBag : FullAuditedEntity<long>
    {

        public virtual string DocumentBagId { get; set; }

    }
}