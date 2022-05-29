using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Dto;
using BTIT.EPM.Lookups;

namespace BTIT.EPM.DigitalSignature
{
    public interface IDocumentRequestsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDocumentRequestForViewDto>> GetAll(GetAllDocumentRequestsInput input);

        Task<GetDocumentRequestForViewDto> GetDocumentRequestForView(long id);

		Task<GetDocumentRequestForViewWithRecipientsDto> GetDocumentRequestForViewWithRecipients(long id);

		Task<CreateOrEditAllRecipientsDto> GetDocumentRequestForEdit(EntityDto<long> input);

		Task SaveOrSend(CreateOrEditAllRecipientsDto input, bool IsSend);

		Task<long> CreateOrEdit(CreateOrEditDocumentRequestDto input);
		Task<GetDocumentRequestAuditTrailsForViewDto> GetDocumentRequestAuditTrail(long id);


		Task Delete(EntityDto<long> input);

		Task<FileDto> GetDocumentRequestsToExcel(GetAllDocumentRequestsForExcelInput input);
		Task SendEmailViewAndSignDocument(int tenantId, string firstName, string lastName, string email,
			long recipientId, string documentTitle, long documentRequestId, bool isSignRequired
			, string recipientCode, bool plainPin , int auditTrailType);


		Task<bool> GeneratePin(ViewAndSignDocumentEmailInput input);
		Task<bool> ConfirmPin(ViewAndSignDocumentEmailInput input);

		Task UpdateDocument(long id, string Code, byte[] Signature, string PinCode = null);

		bool CheckPin(long recipientId, string PinCode);

		Task<int> GetAllDocumentRequestWhichNeedToSignCount();
		Task<PagedResultDto<GetDocumentRequestForViewDto>> GetAllDocumentRequestWhichNeedToSign(GetAllDocumentRequestsInput input);

		Task<string> GetDocumentLink(int DocumentRequestID);
	}
}