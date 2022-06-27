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
        private readonly IRepository<DocumentBag, long> _lookup_documentBagRepository;
        private readonly IRepository<BinaryObject, Guid> _lookup_binaryObjectRepository;
        private readonly IRepository<DocumentBag, long> _documentBagRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly DocumentManager _documentManager;


        public DocumentsAppService(IRepository<Document, long> documentRepository
            , IDocumentsExcelExporter documentsExcelExporter, IRepository<DocumentBag, long> lookup_documentBagRepository
            , IRepository<BinaryObject, Guid> lookup_binaryObjectRepository, IRepository<DocumentBag, long> documentBagRepository
            , IBinaryObjectManager binaryObjectManager
            , DocumentManager documentManager)
        {
            _documentRepository = documentRepository;
            _documentsExcelExporter = documentsExcelExporter;
            _lookup_documentBagRepository = lookup_documentBagRepository;
            _lookup_binaryObjectRepository = lookup_binaryObjectRepository;
            _documentBagRepository = documentBagRepository;
            _binaryObjectManager = binaryObjectManager;
            _documentManager = documentManager;


        }

        public async Task<PagedResultDto<GetDocumentForViewDto>> GetAll(GetAllDocumentsInput input)
        {

            var filteredDocuments = _documentRepository.GetAll()
                        .Include(e => e.DocumentBagFk)
                        .Include(e => e.BinaryObjectFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FileName.Contains(input.Filter) || e.Extension.Contains(input.Filter) || e.ContentType.Contains(input.Filter) /*|| e.Comment.Contains(input.Filter)*/)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileNameFilter), e => e.FileName == input.FileNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExtensionFilter), e => e.Extension == input.ExtensionFilter)
                        .WhereIf(input.MinSizeFilter != null, e => e.Size >= input.MinSizeFilter)
                        .WhereIf(input.MaxSizeFilter != null, e => e.Size <= input.MaxSizeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentTypeFilter), e => e.ContentType == input.ContentTypeFilter);
            
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.DocumentBagDocumentBagIdFilter), e => e.DocumentBagFk != null && e.DocumentBagFk.DocumentBagId == input.DocumentBagDocumentBagIdFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.BinaryObjectDescriptionFilter), e => e.BinaryObjectFk != null && e.BinaryObjectFk.Description == input.BinaryObjectDescriptionFilter)
                        //.WhereIf(input.DocumentBagIdFilter.HasValue, e => false || e.DocumentBagId == input.DocumentBagIdFilter.Value
                        //);

            var pagedAndFilteredDocuments = filteredDocuments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var documents = from o in pagedAndFilteredDocuments
                            join o1 in _lookup_documentBagRepository.GetAll() on o.DocumentBagId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _lookup_binaryObjectRepository.GetAll() on o.BinaryObjectId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            select new GetDocumentForViewDto()
                            {
                                Document = new DocumentDto
                                {
                                    FileName = o.FileName,
                                    Extension = o.Extension,
                                    Size = o.Size,
                                    ContentType = o.ContentType,
                                    //Comment = o.Comment,
                                    //DocumentTypeEnum = o.DocumentTypeEnum,
                                    Id = o.Id
                                },
                                DocumentBagDocumentBagId = s1 == null || s1.DocumentBagId == null ? "" : s1.DocumentBagId.ToString(),
                                BinaryObjectDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                            };

            var totalCount = await filteredDocuments.CountAsync();

            return new PagedResultDto<GetDocumentForViewDto>(
                totalCount,
                await documents.ToListAsync()
            );
        }

        public async Task<GetDocumentForViewDto> GetDocumentForView(long id)
        {
            var document = await _documentRepository.GetAsync(id);

            var output = new GetDocumentForViewDto { Document = ObjectMapper.Map<DocumentDto>(document) };

            if (output.Document.BinaryObjectId != null)
            {
                var _lookupBinaryObject = await _lookup_binaryObjectRepository.FirstOrDefaultAsync((Guid)output.Document.BinaryObjectId);
                output.Document.BinaryObjectBytes = _lookupBinaryObject?.Bytes;
            }

            return output;
        }
        
        public async Task<List<GetDocumentForViewDto>> GetDocuments(long? documentBagId = null, long? documentTypeEnum = null)
        {
            var FilteredDocuments = _documentRepository.GetAll()
                .WhereIf(documentBagId.HasValue, wf => wf.DocumentBagId == documentBagId);
                //.WhereIf(documentTypeEnum.HasValue, wf => wf.DocumentTypeEnum == documentTypeEnum);

            var documents = from o in FilteredDocuments

                            join o2 in _lookup_binaryObjectRepository.GetAll() on o.BinaryObjectId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            select new GetDocumentForViewDto()
                            {
                                Document = new DocumentDto
                                {
                                    FileName = o.FileName,
                                    Extension = o.Extension,
                                    Size = o.Size,
                                    ContentType = o.ContentType,
                                 //   Comment = o.Comment,
                                  //  DocumentTypeEnum = o.DocumentTypeEnum,
                                    Id = o.Id
                                },
                                BinaryObjectDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                            };


            return await documents.ToListAsync();

        }

        [AbpAuthorize(AppPermissions.Pages_Documents_Edit)]
        public async Task<GetDocumentForEditOutput> GetDocumentForEdit(EntityDto<long> input)
        {
            var document = await _documentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDocumentForEditOutput { Document = ObjectMapper.Map<CreateDocumentInputDto>(document) };

            if (output.Document.DocumentBagId != null)
            {
                var _lookupDocumentBag = await _lookup_documentBagRepository.FirstOrDefaultAsync((long)output.Document.DocumentBagId);
                output.DocumentBagDocumentBagId = _lookupDocumentBag?.DocumentBagId?.ToString();
            }

            if (output.Document.BinaryObjectId != null)
            {
                var _lookupBinaryObject = await _lookup_binaryObjectRepository.FirstOrDefaultAsync((Guid)output.Document.BinaryObjectId);
                output.BinaryObjectDescription = _lookupBinaryObject?.Description?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateDocumentInputDto input)
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

        public async Task<long> CreateDocuments(List<CreateDocumentInputDto> filesDto)
        {
            var file = filesDto.FirstOrDefault();

            var fileObject = new BinaryObject(AbpSession.TenantId, file.FileBytes, file.Description);
            await _binaryObjectManager.SaveAsync(fileObject);

            var documentId = await _documentRepository.InsertAndGetIdAsync(new Document
            {
                DocumentBagId = file.DocumentBagId,
               // DocumentTypeEnum = (int)file.DocumentTypeEnum,
                FileName = file.FileName,
                BinaryObjectId = fileObject.Id,
                ContentType = file.ContentType,
              //  Size = file.Size,
               // Comment = file.Description,
                Extension = file.Extension
            });

            return documentId;
        }

        [AbpAuthorize(AppPermissions.Pages_Documents_Create)]
        protected virtual async Task Create(CreateDocumentInputDto input)
        {
            var document = ObjectMapper.Map<Document>(input);

            await _documentRepository.InsertAsync(document);
        }

        [AbpAuthorize(AppPermissions.Pages_Documents_Edit)]
        protected virtual async Task Update(CreateDocumentInputDto input)
        {
            var document = await _documentRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, document);
        }

        [AbpAuthorize(AppPermissions.Pages_Documents_Delete)]
        public async Task Delete(long id)
        {
            await _documentRepository.DeleteAsync(id);
        }

        //public async Task<FileDto> GetDocumentsToExcel(GetAllDocumentsForExcelInput input)
        //{

        //    var filteredDocuments = _documentRepository.GetAll()
        //                .Include(e => e.DocumentBagFk)
        //                .Include(e => e.BinaryObjectFk)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FileName.Contains(input.Filter) || e.Extension.Contains(input.Filter) || e.ContentType.Contains(input.Filter) || e.Comment.Contains(input.Filter))
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.FileNameFilter), e => e.FileName == input.FileNameFilter)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.ExtensionFilter), e => e.Extension == input.ExtensionFilter)
        //                .WhereIf(input.MinSizeFilter != null, e => e.Size >= input.MinSizeFilter)
        //                .WhereIf(input.MaxSizeFilter != null, e => e.Size <= input.MaxSizeFilter)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.ContentTypeFilter), e => e.ContentType == input.ContentTypeFilter)
        //                .WhereIf(input.MinDocumentTypeEnumFilter != null, e => e.DocumentTypeEnum >= input.MinDocumentTypeEnumFilter)
        //                .WhereIf(input.MaxDocumentTypeEnumFilter != null, e => e.DocumentTypeEnum <= input.MaxDocumentTypeEnumFilter)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentBagDocumentBagIdFilter), e => e.DocumentBagFk != null && e.DocumentBagFk.DocumentBagId == input.DocumentBagDocumentBagIdFilter)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.BinaryObjectDescriptionFilter), e => e.BinaryObjectFk != null && e.BinaryObjectFk.Description == input.BinaryObjectDescriptionFilter);

        //    var query = (from o in filteredDocuments
        //                 join o1 in _lookup_documentBagRepository.GetAll() on o.DocumentBagId equals o1.Id into j1
        //                 from s1 in j1.DefaultIfEmpty()

        //                 join o2 in _lookup_binaryObjectRepository.GetAll() on o.BinaryObjectId equals o2.Id into j2
        //                 from s2 in j2.DefaultIfEmpty()

        //                 select new GetDocumentForViewDto()
        //                 {
        //                     Document = new DocumentDto
        //                     {
        //                         FileName = o.FileName,
        //                         Extension = o.Extension,
        //                         Size = o.Size,
        //                         ContentType = o.ContentType,
        //                         Comment = o.Comment,
        //                         DocumentTypeEnum = o.DocumentTypeEnum,
        //                         Id = o.Id
        //                     },
        //                     DocumentBagDocumentBagId = s1 == null || s1.DocumentBagId == null ? "" : s1.DocumentBagId.ToString(),
        //                     BinaryObjectDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
        //                 });

        //    var documentListDtos = await query.ToListAsync();

        //    return _documentsExcelExporter.ExportToFile(documentListDtos);
        //}

        [AbpAuthorize(AppPermissions.Pages_Documents)]
        public async Task<List<DocumentDocumentBagLookupTableDto>> GetAllDocumentBagForTableDropdown()
        {
            return await _lookup_documentBagRepository.GetAll()
                .Select(documentBag => new DocumentDocumentBagLookupTableDto
                {
                    Id = documentBag.Id,
                    DisplayName = documentBag == null || documentBag.DocumentBagId == null ? "" : documentBag.DocumentBagId.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Documents)]
        public async Task<List<DocumentBinaryObjectLookupTableDto>> GetAllBinaryObjectForTableDropdown()
        {
            return await _lookup_binaryObjectRepository.GetAll()
                .Select(binaryObject => new DocumentBinaryObjectLookupTableDto
                {
                    Id = binaryObject.Id.ToString(),
                    DisplayName = binaryObject == null || binaryObject.Description == null ? "" : binaryObject.Description.ToString()
                }).ToListAsync();
        }
        public long GenerateDocmantBagId()
        {
            var documentBagId = _documentBagRepository.InsertAndGetId(new DocumentBag());
            return documentBagId;
        }

        Task<FileDto> IDocumentsAppService.GetDocumentsToExcel(GetAllDocumentsForExcelInput input)
        {
            throw new NotImplementedException();
        }
    }
}