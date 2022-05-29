using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using BTIT.EPM.Authorization;
using BTIT.EPM.Authorization.Users;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.DigitalSignature.Exporting;
using BTIT.EPM.Documents;
using BTIT.EPM.Dto;
using BTIT.EPM.Lookups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace BTIT.EPM.DigitalSignature
{
    public class DocumentRequestAuditTrailsAppService : EPMAppServiceBase, IDocumentRequestAuditTrailsAppService
    {
        private readonly IRepository<DocumentRequestAuditTrail, long> _documentRequestAuditTrailRepository;
        private readonly IDocumentRequestAuditTrailsExcelExporter _documentRequestAuditTrailsExcelExporter;
        private readonly IRepository<DocumentRequest, long> _lookup_documentRequestRepository;
        private readonly IRepository<Recipient, long> _lookup_recipientRepository;
        private readonly IRepository<Document, long> _documentRepository;
        private readonly IRepository<User, long> _userRepository;

        public DocumentRequestAuditTrailsAppService(IRepository<DocumentRequestAuditTrail, long> documentRequestAuditTrailRepository,
            IDocumentRequestAuditTrailsExcelExporter documentRequestAuditTrailsExcelExporter,
            IRepository<DocumentRequest, long> lookup_documentRequestRepository,
            IRepository<Recipient, long> lookup_recipientRepository,
            IRepository<Document, long> documentRepository,
            IRepository<User, long> userRepository)
        {
            _documentRequestAuditTrailRepository = documentRequestAuditTrailRepository;
            _documentRequestAuditTrailsExcelExporter = documentRequestAuditTrailsExcelExporter;
            _lookup_documentRequestRepository = lookup_documentRequestRepository;
            _lookup_recipientRepository = lookup_recipientRepository;
            _documentRepository = documentRepository;
            _userRepository = userRepository;
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequestAuditTrails)]
        public async Task<PagedResultDto<GetDocumentRequestAuditTrailForViewDto>> GetAll(GetAllDocumentRequestAuditTrailsInput input)
        {
            var typeFilter = (AuditTrailType)input.TypeFilter;

            var filteredDocumentRequestAuditTrails = _documentRequestAuditTrailRepository.GetAll()
                        .Include(e => e.DocumentRequestFk)
                        .Include(e => e.RecipientFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ClientIpAddress.Contains(input.Filter))
                        .WhereIf(input.TypeFilter > -1, e => e.Type == typeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ClientIpAddressFilter), e => e.ClientIpAddress == input.ClientIpAddressFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentRequestDocumentTitleFilter), e => e.DocumentRequestFk != null && e.DocumentRequestFk.DocumentTitle == input.DocumentRequestDocumentTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecipientFirstNameFilter), e => e.RecipientFk != null && e.RecipientFk.FirstName == input.RecipientFirstNameFilter);

            var pagedAndFilteredDocumentRequestAuditTrails = filteredDocumentRequestAuditTrails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var documentRequestAuditTrails = from o in pagedAndFilteredDocumentRequestAuditTrails
                                             join o1 in _lookup_documentRequestRepository.GetAll() on o.DocumentRequestId equals o1.Id into j1
                                             from s1 in j1.DefaultIfEmpty()

                                             join o2 in _lookup_recipientRepository.GetAll() on o.RecipientId equals o2.Id into j2
                                             from s2 in j2.DefaultIfEmpty()

                                             select new GetDocumentRequestAuditTrailForViewDto()
                                             {
                                                 DocumentRequestAuditTrail = new DocumentRequestAuditTrailDto
                                                 {
                                                     Type = o.Type,
                                                     ClientIpAddress = o.ClientIpAddress,
                                                     Id = o.Id
                                                 },
                                                 DocumentRequestDocumentTitle = s1 == null || s1.DocumentTitle.IsNullOrEmpty() ? "" : s1.DocumentTitle.ToString(),
                                                 RecipientFirstName = s2 == null || s2.FirstName.IsNullOrEmpty() ? "" : s2.FirstName.ToString()
                                             };

            var totalCount = await filteredDocumentRequestAuditTrails.CountAsync();

            return new PagedResultDto<GetDocumentRequestAuditTrailForViewDto>(
                totalCount,
                await documentRequestAuditTrails.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequestAuditTrails)]
        public async Task<DocumentAuditTrailDto> GetDocumentRequestAuditTrailForView(long documentRequestId, long documentId)
        {
            var documentRequestAuditTrails = await _documentRequestAuditTrailRepository.GetAll().Where(x => x.DocumentRequestId == documentRequestId)
                .Include(x => x.DocumentRequestFk).ThenInclude(r => r.Recipients).ThenInclude(u => u.UserFk).ToListAsync();
            var document = _documentRepository.Get(documentId);

            var output = new DocumentAuditTrailDto { DocumentRequestAuditTrails = ObjectMapper.Map<List<DocumentRequestAuditTrailDto>>(documentRequestAuditTrails), DocumentRequestTitle = documentRequestAuditTrails?.FirstOrDefault()?.DocumentRequestFk?.DocumentTitle ?? "" };
            output.Document = ObjectMapper.Map<Documents.Dtos.DocumentDto>(document);
            foreach (var item in output.DocumentRequestAuditTrails)
            {
                var audit = documentRequestAuditTrails.FirstOrDefault(i => i.Id == item.Id);
                item.CreationDate = audit.CreationTime.ToString("dddd, dd MMMM yyyy HH:mm:ss");

                if (item.Type == AuditTrailType.Sent)
                {
                    documentRequestAuditTrails.FirstOrDefault().DocumentRequestFk.Recipients
                        .DistinctBy(x => x.UserId).ToList().ForEach(r =>
                        {
                            if (r.Type == RecipientType.External)
                                item.UserName += r.FirstName + " , ";
                            else
                                item.UserName += r.UserFk?.Name ?? "" + " , ";
                        });
                }
                else
                {
                    var user = _userRepository.Get(audit.CreatorUserId.Value);
                    item.UserName = user.Name;
                    item.UserEmail = user.EmailAddress;
                }
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequestAuditTrails_Edit)]
        public async Task<GetDocumentRequestAuditTrailForEditOutput> GetDocumentRequestAuditTrailForEdit(EntityDto<long> input)
        {
            var documentRequestAuditTrail = await _documentRequestAuditTrailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDocumentRequestAuditTrailForEditOutput { DocumentRequestAuditTrail = ObjectMapper.Map<CreateOrEditDocumentRequestAuditTrailDto>(documentRequestAuditTrail) };

            if (output.DocumentRequestAuditTrail.DocumentRequestId != null)
            {
                var _lookupDocumentRequest = await _lookup_documentRequestRepository.FirstOrDefaultAsync((long)output.DocumentRequestAuditTrail.DocumentRequestId);
                output.DocumentRequestDocumentTitle = _lookupDocumentRequest.DocumentTitle.ToString();
            }

            if (output.DocumentRequestAuditTrail.RecipientId != null)
            {
                var _lookupRecipient = await _lookup_recipientRepository.FirstOrDefaultAsync((long)output.DocumentRequestAuditTrail.RecipientId);
                output.RecipientFirstName = _lookupRecipient.FirstName.ToString();
            }

            return output;
        }

        [AbpAuthorize()]
        public async Task AddAuditByDocumentRequestIdAsync(long documentRequestId, AuditTrailType type, long? recipientId = null)
        {
            var audit = new DocumentRequestAuditTrail
            {
                DocumentRequestId = documentRequestId,
                RecipientId = recipientId,
                Type = type
            };

            if (AbpSession.TenantId != null)
            {
                audit.TenantId = (int)AbpSession.TenantId;
            }
            await _documentRequestAuditTrailRepository.InsertAsync(audit);
        }

        public async Task CreateOrEdit(CreateOrEditDocumentRequestAuditTrailDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequestAuditTrails_Create)]
        protected virtual async Task Create(CreateOrEditDocumentRequestAuditTrailDto input)
        {
            var documentRequestAuditTrail = ObjectMapper.Map<DocumentRequestAuditTrail>(input);


            if (AbpSession.TenantId != null)
            {
                documentRequestAuditTrail.TenantId = (int)AbpSession.TenantId;
            }


            await _documentRequestAuditTrailRepository.InsertAsync(documentRequestAuditTrail);
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequestAuditTrails_Edit)]
        protected virtual async Task Update(CreateOrEditDocumentRequestAuditTrailDto input)
        {
            var documentRequestAuditTrail = await _documentRequestAuditTrailRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, documentRequestAuditTrail);
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequestAuditTrails_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _documentRequestAuditTrailRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequestAuditTrails)]
        public async Task<FileDto> GetDocumentRequestAuditTrailsToExcel(GetAllDocumentRequestAuditTrailsForExcelInput input)
        {
            var typeFilter = (AuditTrailType)input.TypeFilter;

            var filteredDocumentRequestAuditTrails = _documentRequestAuditTrailRepository.GetAll()
                        .Include(e => e.DocumentRequestFk)
                        .Include(e => e.RecipientFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ClientIpAddress.Contains(input.Filter))
                        .WhereIf(input.TypeFilter > -1, e => e.Type == typeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ClientIpAddressFilter), e => e.ClientIpAddress == input.ClientIpAddressFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentRequestDocumentTitleFilter), e => e.DocumentRequestFk != null && e.DocumentRequestFk.DocumentTitle == input.DocumentRequestDocumentTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecipientFirstNameFilter), e => e.RecipientFk != null && e.RecipientFk.FirstName == input.RecipientFirstNameFilter);

            var query = (from o in filteredDocumentRequestAuditTrails
                         join o1 in _lookup_documentRequestRepository.GetAll() on o.DocumentRequestId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_recipientRepository.GetAll() on o.RecipientId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetDocumentRequestAuditTrailForViewDto()
                         {
                             DocumentRequestAuditTrail = new DocumentRequestAuditTrailDto
                             {
                                 Type = o.Type,
                                 ClientIpAddress = o.ClientIpAddress,
                                 Id = o.Id
                             },
                             DocumentRequestDocumentTitle = s1 == null ? "" : s1.DocumentTitle.ToString(),
                             RecipientFirstName = s2 == null ? "" : s2.FirstName.ToString()
                         });


            var documentRequestAuditTrailListDtos = await query.ToListAsync();

            return _documentRequestAuditTrailsExcelExporter.ExportToFile(documentRequestAuditTrailListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_DocumentRequestAuditTrails)]
        public async Task<List<DocumentRequestAuditTrailDocumentRequestLookupTableDto>> GetAllDocumentRequestForTableDropdown()
        {
            return await _lookup_documentRequestRepository.GetAll()
                .Select(documentRequest => new DocumentRequestAuditTrailDocumentRequestLookupTableDto
                {
                    Id = documentRequest.Id,
                    DisplayName = documentRequest.DocumentTitle.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequestAuditTrails)]
        public async Task<List<DocumentRequestAuditTrailRecipientLookupTableDto>> GetAllRecipientForTableDropdown()
        {
            return await _lookup_recipientRepository.GetAll()
                .Select(recipient => new DocumentRequestAuditTrailRecipientLookupTableDto
                {
                    Id = recipient.Id,
                    DisplayName = recipient.FirstName.ToString()
                }).ToListAsync();
        }

    }
}