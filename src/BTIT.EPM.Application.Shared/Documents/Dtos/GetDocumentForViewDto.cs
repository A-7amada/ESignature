namespace BTIT.EPM.Documents.Dtos
{
    public class GetDocumentForViewDto
    {
		public DocumentDto Document { get; set; }

		public string BinaryObjectTenantId { get; set;}

		public string DocumentRequestDocumentTitle { get; set;}

        public string DocumentBagDocumentBagId { get; set; }

        public string BinaryObjectDescription { get; set; }
        public string FileUrl { get; set; }
    }
}