using System.Linq;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class GetDocumentRequestForViewWithRecipientsDto
    {
		public DocumentRequestDto DocumentRequest { get; set; }

        public IQueryable<RecipientDto> Recipients { get; set; }

    }
}