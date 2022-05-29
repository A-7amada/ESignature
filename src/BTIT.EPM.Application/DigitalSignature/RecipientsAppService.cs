using BTIT.EPM.Authorization.Users;
					using System.Collections.Generic;
using BTIT.EPM.DigitalSignature;
					using System.Collections.Generic;

using BTIT.EPM.Lookups;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using BTIT.EPM.DigitalSignature.Exporting;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Dto;
using Abp.Application.Services.Dto;
using BTIT.EPM.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Abp.UI;
using Abp.Domain.Uow;
using Abp.Application.Services;

namespace BTIT.EPM.DigitalSignature
{

    public class RecipientsAppService : EPMAppServiceBase, IRecipientsAppService
    {
		 private readonly IRepository<Recipient, long> _recipientRepository;
		 private readonly IRecipientsExcelExporter _recipientsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<DocumentRequest,long> _lookup_documentRequestRepository;
		private readonly IDocumentRequestAuditTrailsAppService _documentRequestAuditTrailsAppService;
		private readonly IRepository<DocumentRequestAuditTrail, long> _documentRequestAuditTrailRepository;
		private readonly IUnitOfWorkManager _unitOfWorkManager;



		public RecipientsAppService(IRepository<Recipient, long> recipientRepository, IRecipientsExcelExporter recipientsExcelExporter,
			IRepository<User, long> lookup_userRepository, IRepository<DocumentRequest, long> lookup_documentRequestRepository,
								IDocumentRequestAuditTrailsAppService documentRequestAuditTrailsAppService, 
								IRepository<DocumentRequestAuditTrail, long> documentRequestAuditTrailRepository,
								IUnitOfWorkManager unitOfWorkManager)
		{
			_recipientRepository = recipientRepository;
			_recipientsExcelExporter = recipientsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
			_lookup_documentRequestRepository = lookup_documentRequestRepository;
			_documentRequestAuditTrailsAppService = documentRequestAuditTrailsAppService;
			_documentRequestAuditTrailRepository = documentRequestAuditTrailRepository;
			_unitOfWorkManager = unitOfWorkManager;
		}

		[AbpAuthorize(AppPermissions.Pages_Recipients)]
		public async Task<PagedResultDto<GetRecipientForViewDto>> GetAll(GetAllRecipientsInput input)
         {
			var typeFilter = (RecipientType) input.TypeFilter;
			
			var filteredRecipients = _recipientRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.DocumentRequestFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.SignerPin.Contains(input.Filter) || e.FieldName.Contains(input.Filter) || e.MobileNumber.Contains(input.Filter))
						.WhereIf(input.TypeFilter > -1, e => e.Type == typeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter),  e => e.FirstName == input.FirstNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter),  e => e.LastName == input.LastNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter),  e => e.Email == input.EmailFilter)
						.WhereIf(input.IsSignerFilter > -1,  e => (input.IsSignerFilter == 1 && e.IsSigner) || (input.IsSignerFilter == 0 && !e.IsSigner) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter.ToString()),  e => e.Code.ToString() == input.CodeFilter.ToString())
						.WhereIf(input.MinViewDateFilter != null, e => e.ViewDate >= input.MinViewDateFilter)
						.WhereIf(input.MaxViewDateFilter != null, e => e.ViewDate <= input.MaxViewDateFilter)
						.WhereIf(input.MinSignatureDateFilter != null, e => e.SignatureDate >= input.MinSignatureDateFilter)
						.WhereIf(input.MaxSignatureDateFilter != null, e => e.SignatureDate <= input.MaxSignatureDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SignerPinFilter),  e => e.SignerPin == input.SignerPinFilter)
						.WhereIf(input.IsSignedFilter > -1,  e => (input.IsSignedFilter == 1 && e.IsSigned) || (input.IsSignedFilter == 0 && !e.IsSigned) )
						.WhereIf(input.MinSigneOrderFilter != null, e => e.SigneOrder >= input.MinSigneOrderFilter)
						.WhereIf(input.MaxSigneOrderFilter != null, e => e.SigneOrder <= input.MaxSigneOrderFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FieldNameFilter),  e => e.FieldName == input.FieldNameFilter)
						.WhereIf(input.MinSentDateFilter != null, e => e.SentDate >= input.MinSentDateFilter)
						.WhereIf(input.MaxSentDateFilter != null, e => e.SentDate <= input.MaxSentDateFilter)
						.WhereIf(input.IsSentFilter > -1,  e => (input.IsSentFilter == 1 && e.IsSent) || (input.IsSentFilter == 0 && !e.IsSent) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DocumentRequestDocumentTitleFilter), e => e.DocumentRequestFk != null && e.DocumentRequestFk.DocumentTitle == input.DocumentRequestDocumentTitleFilter);

			var pagedAndFilteredRecipients = filteredRecipients
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var recipients = from o in pagedAndFilteredRecipients
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_documentRequestRepository.GetAll() on o.DocumentRequestId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetRecipientForViewDto() {
							Recipient = new RecipientDto
							{
                                Type = o.Type,
                                FirstName = o.FirstName,
                                LastName = o.LastName,
                                Email = o.Email,
                                IsSigner = o.IsSigner,
                                Code = o.Code,
                                ViewDate = o.ViewDate,
                                SignatureDate = o.SignatureDate,
                                SignerPin = o.SignerPin,
                                IsSigned = o.IsSigned,
                                SigneOrder = o.SigneOrder,
                                FieldName = o.FieldName,
                                SentDate = o.SentDate,
                                IsSent = o.IsSent,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	DocumentRequestDocumentTitle = s2 == null ? "" : s2.DocumentTitle.ToString()
						};

            var totalCount = await filteredRecipients.CountAsync();

            return new PagedResultDto<GetRecipientForViewDto>(
                totalCount,
                await recipients.ToListAsync()
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Recipients)]
		public async Task<GetRecipientForViewDto> GetRecipientForView(long id)
         {
            var recipient = await _recipientRepository.GetAsync(id);

            var output = new GetRecipientForViewDto { Recipient = ObjectMapper.Map<RecipientDto>(recipient) };

		    if (output.Recipient.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Recipient.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.Recipient.DocumentRequestId != null)
            {
                var _lookupDocumentRequest = await _lookup_documentRequestRepository.FirstOrDefaultAsync((long)output.Recipient.DocumentRequestId);
                output.DocumentRequestDocumentTitle = _lookupDocumentRequest.DocumentTitle.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Recipients_Edit)]
		 public async Task<GetRecipientForEditOutput> GetRecipientForEdit(EntityDto<long> input)
         {
            var recipient = await _recipientRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRecipientForEditOutput {Recipient = ObjectMapper.Map<CreateOrEditRecipientDto>(recipient)};

		    if (output.Recipient.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Recipient.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.Recipient.DocumentRequestId != null)
            {
                var _lookupDocumentRequest = await _lookup_documentRequestRepository.FirstOrDefaultAsync((long)output.Recipient.DocumentRequestId);
                output.DocumentRequestDocumentTitle = _lookupDocumentRequest.DocumentTitle.ToString();
            }
			
            return output;
         }

		[AbpAuthorize(AppPermissions.Pages_Recipients)]
		public async Task CreateOrEdit(CreateOrEditRecipientDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Recipients_Create)]
		 protected virtual async Task Create(CreateOrEditRecipientDto input)
         {
            var recipient = ObjectMapper.Map<Recipient>(input);

			
			if (AbpSession.TenantId != null)
			{
				recipient.TenantId = (int) AbpSession.TenantId;
			}
		

            await _recipientRepository.InsertAsync(recipient);
         }

		 [AbpAuthorize(AppPermissions.Pages_Recipients_Edit)]
		 protected virtual async Task Update(CreateOrEditRecipientDto input)
         {
            var recipient = await _recipientRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, recipient);
         }

		 [AbpAuthorize(AppPermissions.Pages_Recipients_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _recipientRepository.DeleteAsync(input.Id);
         }

		[AbpAuthorize(AppPermissions.Pages_Recipients)]
		public async Task<FileDto> GetRecipientsToExcel(GetAllRecipientsForExcelInput input)
         {
			var typeFilter = (RecipientType) input.TypeFilter;
			
			var filteredRecipients = _recipientRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.DocumentRequestFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.SignerPin.Contains(input.Filter) || e.FieldName.Contains(input.Filter) || e.MobileNumber.Contains(input.Filter))
						.WhereIf(input.TypeFilter > -1, e => e.Type == typeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter),  e => e.FirstName == input.FirstNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter),  e => e.LastName == input.LastNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter),  e => e.Email == input.EmailFilter)
						.WhereIf(input.IsSignerFilter > -1,  e => (input.IsSignerFilter == 1 && e.IsSigner) || (input.IsSignerFilter == 0 && !e.IsSigner) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter.ToString()),  e => e.Code.ToString() == input.CodeFilter.ToString())
						.WhereIf(input.MinViewDateFilter != null, e => e.ViewDate >= input.MinViewDateFilter)
						.WhereIf(input.MaxViewDateFilter != null, e => e.ViewDate <= input.MaxViewDateFilter)
						.WhereIf(input.MinSignatureDateFilter != null, e => e.SignatureDate >= input.MinSignatureDateFilter)
						.WhereIf(input.MaxSignatureDateFilter != null, e => e.SignatureDate <= input.MaxSignatureDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SignerPinFilter),  e => e.SignerPin == input.SignerPinFilter)
						.WhereIf(input.IsSignedFilter > -1,  e => (input.IsSignedFilter == 1 && e.IsSigned) || (input.IsSignedFilter == 0 && !e.IsSigned) )
						.WhereIf(input.MinSigneOrderFilter != null, e => e.SigneOrder >= input.MinSigneOrderFilter)
						.WhereIf(input.MaxSigneOrderFilter != null, e => e.SigneOrder <= input.MaxSigneOrderFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FieldNameFilter),  e => e.FieldName == input.FieldNameFilter)
						.WhereIf(input.MinSentDateFilter != null, e => e.SentDate >= input.MinSentDateFilter)
						.WhereIf(input.MaxSentDateFilter != null, e => e.SentDate <= input.MaxSentDateFilter)
						.WhereIf(input.IsSentFilter > -1,  e => (input.IsSentFilter == 1 && e.IsSent) || (input.IsSentFilter == 0 && !e.IsSent) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DocumentRequestDocumentTitleFilter), e => e.DocumentRequestFk != null && e.DocumentRequestFk.DocumentTitle == input.DocumentRequestDocumentTitleFilter);

			var query = (from o in filteredRecipients
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_documentRequestRepository.GetAll() on o.DocumentRequestId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetRecipientForViewDto() { 
							Recipient = new RecipientDto
							{
                                Type = o.Type,
                                FirstName = o.FirstName,
                                LastName = o.LastName,
                                Email = o.Email,
                                IsSigner = o.IsSigner,
                                Code = o.Code,
                                ViewDate = o.ViewDate,
                                SignatureDate = o.SignatureDate,
                                SignerPin = o.SignerPin,
                                IsSigned = o.IsSigned,
                                SigneOrder = o.SigneOrder,
                                FieldName = o.FieldName,
                                SentDate = o.SentDate,
                                IsSent = o.IsSent,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	DocumentRequestDocumentTitle = s2 == null ? "" : s2.DocumentTitle.ToString()
						 });


            var recipientListDtos = await query.ToListAsync();

            return _recipientsExcelExporter.ExportToFile(recipientListDtos);
         }


			[AbpAuthorize(AppPermissions.Pages_Recipients)]
			public async Task<List<RecipientUserLookupTableDto>> GetAllUserForTableDropdown()
			{
				return await _lookup_userRepository.GetAll()
					.Select(user => new RecipientUserLookupTableDto
					{
						Id = user.Id,
						DisplayName = user.Name.ToString()
					}).ToListAsync();
			}
							
			[AbpAuthorize(AppPermissions.Pages_Recipients)]
			public async Task<List<RecipientDocumentRequestLookupTableDto>> GetAllDocumentRequestForTableDropdown()
			{
				return await _lookup_documentRequestRepository.GetAll()
					.Select(documentRequest => new RecipientDocumentRequestLookupTableDto
					{
						Id = documentRequest.Id,
						DisplayName = documentRequest.DocumentTitle.ToString()
					}).ToListAsync();
			}


		[AllowAnonymous]
		[RemoteService(false)]
		public async Task RecipientViewDocument(long id)
		{

			var recipient = await _recipientRepository.GetAsync(id);

			await _documentRequestAuditTrailRepository.InsertAndGetIdAsync(new DocumentRequestAuditTrail
			{
				DocumentRequestId = recipient.DocumentRequestId.Value,
				RecipientId = recipient.Id,
				Type = AuditTrailType.Viewed
			});
			recipient.ViewDate = DateTime.Now;

		}
	}
}