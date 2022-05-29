using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using BTIT.EPM.Authorization;
using BTIT.EPM.DigitalSignature;
using BTIT.EPM.Documents.Dtos;
using BTIT.EPM.Documents.Exporting;
using BTIT.EPM.Dto;
using BTIT.EPM.Lookups;
using BTIT.EPM.Storage;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace BTIT.EPM.Documents
{
    [AbpAuthorize(AppPermissions.Pages_Documents)]
    public class DocumentsAppService : EPMAppServiceBase, IDocumentsAppService
    {
        private readonly IRepository<Document, long> _documentRepository;
        private readonly IDocumentsExcelExporter _documentsExcelExporter;
        private readonly IRepository<BinaryObject, Guid> _lookup_binaryObjectRepository;
        private readonly IRepository<DocumentRequest, long> _lookup_documentRequestRepository;
        private readonly IRepository<Recipient, long> _recipientsRepository;

        public DocumentsAppService(IRepository<Document, long> documentRepository, IDocumentsExcelExporter documentsExcelExporter, IRepository<BinaryObject, Guid> lookup_binaryObjectRepository, IRepository<DocumentRequest, long> lookup_documentRequestRepository, IRepository<Recipient, long> recipientsRepository)
        {
            _documentRepository = documentRepository;
            _documentsExcelExporter = documentsExcelExporter;
            _lookup_binaryObjectRepository = lookup_binaryObjectRepository;
            _lookup_documentRequestRepository = lookup_documentRequestRepository;
            _recipientsRepository = recipientsRepository;
        }

        public async Task<PagedResultDto<GetDocumentForViewDto>> GetAll(GetAllDocumentsInput input)
        {

            var filteredDocuments = _documentRepository.GetAll()
                        .Include(e => e.BinaryObjectFk)
                        .Include(e => e.DocumentRequestFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FileName.Contains(input.Filter) || e.Extension.Contains(input.Filter) || e.ContentType.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileNameFilter), e => e.FileName == input.FileNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExtensionFilter), e => e.Extension == input.ExtensionFilter)
                        .WhereIf(input.MinSizeFilter != null, e => e.Size >= input.MinSizeFilter)
                        .WhereIf(input.MaxSizeFilter != null, e => e.Size <= input.MaxSizeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentTypeFilter), e => e.ContentType == input.ContentTypeFilter)
                        .WhereIf(input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentRequestDocumentTitleFilter), e => e.DocumentRequestFk != null && e.DocumentRequestFk.DocumentTitle == input.DocumentRequestDocumentTitleFilter);

            var pagedAndFilteredDocuments = filteredDocuments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var documents = from o in pagedAndFilteredDocuments
                            join o1 in _lookup_binaryObjectRepository.GetAll() on o.BinaryObjectId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _lookup_documentRequestRepository.GetAll() on o.DocumentRequestId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            select new GetDocumentForViewDto()
                            {
                                Document = new DocumentDto
                                {
                                    FileName = o.FileName,
                                    Extension = o.Extension,
                                    Size = o.Size,
                                    ContentType = o.ContentType,
                                    IsActive = o.IsActive,
                                    Id = o.Id
                                },
                                BinaryObjectTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                                DocumentRequestDocumentTitle = s2 == null ? "" : s2.DocumentTitle.ToString()
                            };

            var totalCount = await filteredDocuments.CountAsync();

            return new PagedResultDto<GetDocumentForViewDto>(
                totalCount,
                await documents.ToListAsync()
            );
        }

        public async Task<PagedResultDto<GetDocumentForViewDto>> GetUserDocuments(GetAllDocumentsInput input)
        {
            var currentUserId = AbpSession.UserId;
            var filteredDocuments = _documentRepository.GetAll()
            .Include(e => e.BinaryObjectFk)
            .Include(e => e.DocumentRequestFk).ThenInclude(x => x.Recipients).ThenInclude(x => x.UserFk)
            .Where(x => x.CreatorUserId == currentUserId || (x.DocumentRequestFk != null && x.DocumentRequestFk.Recipients != null && x.DocumentRequestFk.Recipients.Select(r => r.UserId).Contains(currentUserId)))
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FileName.Contains(input.Filter) || e.Extension.Contains(input.Filter) || e.ContentType.Contains(input.Filter))
            .WhereIf(!string.IsNullOrWhiteSpace(input.FileNameFilter), e => e.FileName == input.FileNameFilter)
            .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentRequestDocumentTitleFilter), e => e.DocumentRequestFk != null && e.DocumentRequestFk.DocumentTitle == input.DocumentRequestDocumentTitleFilter);

            filteredDocuments.ToList().ForEach(x =>
            {
                x.Recipients = x.DocumentRequestFk.Recipients != null ? _getRecipients(x.DocumentRequestFk.Recipients.DistinctBy(u => u.UserId).ToList()) : "";
            });

            var pagedAndFilteredDocuments = filteredDocuments
              .OrderBy(input.Sorting ?? "id asc")
              .PageBy(input);

            var documents = from o in pagedAndFilteredDocuments
                            join o1 in _lookup_binaryObjectRepository.GetAll() on o.BinaryObjectId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _lookup_documentRequestRepository.GetAll() on o.DocumentRequestId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            select new GetDocumentForViewDto()
                            {
                                Document = new DocumentDto
                                {
                                    FileName = o.FileName,
                                    CreatedDate = o.CreationTime.ToShortDateString(),
                                    Status = s2.Status,
                                    Recipients = o.Recipients,
                                    Id = o.Id,
                                    DocumentRequestId = o.DocumentRequestId
                                },
                                BinaryObjectTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                                DocumentRequestDocumentTitle = s2 == null ? "" : s2.DocumentTitle
                            };

            var totalCount = await filteredDocuments.CountAsync();

            return new PagedResultDto<GetDocumentForViewDto>(
                totalCount,
                documents.ToList()
            );
        }

        public async Task<GetDocumentForViewDto> GetDocumentForView(long id)
        {
            var document = await _documentRepository.GetAsync(id);

            var output = new GetDocumentForViewDto { Document = ObjectMapper.Map<DocumentDto>(document) };

            if (output.Document.BinaryObjectId != null)
            {
                var _lookupBinaryObject = await _lookup_binaryObjectRepository.FirstOrDefaultAsync((Guid)output.Document.BinaryObjectId);
                output.BinaryObjectTenantId = _lookupBinaryObject.TenantId.ToString();
            }

            if (output.Document.DocumentRequestId != null)
            {
                var _lookupDocumentRequest = await _lookup_documentRequestRepository.FirstOrDefaultAsync((long)output.Document.DocumentRequestId);
                output.DocumentRequestDocumentTitle = _lookupDocumentRequest.DocumentTitle.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Documents_Edit)]
        public async Task<GetDocumentForEditOutput> GetDocumentForEdit(EntityDto<long> input)
        {
            var document = await _documentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDocumentForEditOutput { Document = ObjectMapper.Map<CreateOrEditDocumentDto>(document) };

            if (output.Document.BinaryObjectId != null)
            {
                var _lookupBinaryObject = await _lookup_binaryObjectRepository.FirstOrDefaultAsync((Guid)output.Document.BinaryObjectId);
                output.BinaryObjectTenantId = _lookupBinaryObject.TenantId.ToString();
            }

            if (output.Document.DocumentRequestId != null)
            {
                var _lookupDocumentRequest = await _lookup_documentRequestRepository.FirstOrDefaultAsync((long)output.Document.DocumentRequestId);
                output.DocumentRequestDocumentTitle = _lookupDocumentRequest.DocumentTitle.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDocumentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Documents_Create)]
        protected virtual async Task Create(CreateOrEditDocumentDto input)
        {
            var document = ObjectMapper.Map<Document>(input);


            if (AbpSession.TenantId != null)
            {
                document.TenantId = (int)AbpSession.TenantId;
            }


            await _documentRepository.InsertAsync(document);
        }

        [AbpAuthorize(AppPermissions.Pages_Documents_Edit)]
        protected virtual async Task Update(CreateOrEditDocumentDto input)
        {
            var document = await _documentRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, document);
        }

        [AbpAuthorize(AppPermissions.Pages_Documents_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _documentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDocumentsToExcel(GetAllDocumentsForExcelInput input)
        {

            var filteredDocuments = _documentRepository.GetAll()
                        .Include(e => e.BinaryObjectFk)
                        .Include(e => e.DocumentRequestFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FileName.Contains(input.Filter) || e.Extension.Contains(input.Filter) || e.ContentType.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileNameFilter), e => e.FileName == input.FileNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExtensionFilter), e => e.Extension == input.ExtensionFilter)
                        .WhereIf(input.MinSizeFilter != null, e => e.Size >= input.MinSizeFilter)
                        .WhereIf(input.MaxSizeFilter != null, e => e.Size <= input.MaxSizeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentTypeFilter), e => e.ContentType == input.ContentTypeFilter)
                        .WhereIf(input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentRequestDocumentTitleFilter), e => e.DocumentRequestFk != null && e.DocumentRequestFk.DocumentTitle == input.DocumentRequestDocumentTitleFilter);

            var query = (from o in filteredDocuments
                         join o1 in _lookup_binaryObjectRepository.GetAll() on o.BinaryObjectId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_documentRequestRepository.GetAll() on o.DocumentRequestId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetDocumentForViewDto()
                         {
                             Document = new DocumentDto
                             {
                                 FileName = o.FileName,
                                 Extension = o.Extension,
                                 Size = o.Size,
                                 ContentType = o.ContentType,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             },
                             BinaryObjectTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                             DocumentRequestDocumentTitle = s2 == null ? "" : s2.DocumentTitle.ToString()
                         });


            var documentListDtos = await query.ToListAsync();

            return _documentsExcelExporter.ExportToFile(documentListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_Documents)]
        public async Task<List<DocumentBinaryObjectLookupTableDto>> GetAllBinaryObjectForTableDropdown()
        {
            return await _lookup_binaryObjectRepository.GetAll()
                .Select(binaryObject => new DocumentBinaryObjectLookupTableDto
                {
                    Id = binaryObject.Id.ToString(),
                    DisplayName = binaryObject.TenantId.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Documents)]
        public async Task<List<DocumentDocumentRequestLookupTableDto>> GetAllDocumentRequestForTableDropdown()
        {
            return await _lookup_documentRequestRepository.GetAll()
                .Select(documentRequest => new DocumentDocumentRequestLookupTableDto
                {
                    Id = documentRequest.Id,
                    DisplayName = documentRequest.DocumentTitle.ToString()
                }).ToListAsync();
        }

        public async Task<DocumentDto> GetDocument(long documentId)
        {
            var document = await _documentRepository.GetAsync(documentId);
            var binaryObject = await _lookup_binaryObjectRepository.GetAsync(document.BinaryObjectId.Value);
            
            return new DocumentDto
            {
                FileName = document?.FileName,
                FileBytes = binaryObject?.Bytes ?? new byte[0],
                ContentType = document?.ContentType,
            };
        }

        private string _getRecipients(List<Recipient> recipients)
        {
            var result = "";
            recipients.ForEach(r =>
            {
                if (r.Type == RecipientType.External)
                    result += r.FirstName + " " + r.LastName + " , ";
                else
                    result += r.UserFk.Name + " , ";
            });

            return result;
        }
    }
}