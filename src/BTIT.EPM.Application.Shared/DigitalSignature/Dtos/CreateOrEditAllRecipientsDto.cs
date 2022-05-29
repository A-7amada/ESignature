using System;
using System.Collections.Generic;
using System.Text;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class CreateOrEditAllRecipientsDto
    {
        public bool IsSigningOrdered { get; set; }
        public long? DocumentRequestId { get; set; }
        public Guid BinaryObjectId { get; set; }
        public string BinaryObjectContentType { get; set; }
        public string DocumentTitle { get; set; }
        public string MessageBody { get; set; }
        public List<CreateOrEditRecipientDto> Recipients { get; set; }
    }
}
