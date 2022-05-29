using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Net.Mail;
using Abp.Runtime.Session;
using Abp.UI;
using BTIT.EPM.Authorization;
using BTIT.EPM.Authorization.Users;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.DigitalSignature.Exporting;
using BTIT.EPM.Documents;
using BTIT.EPM.Dto;
using BTIT.EPM.Lookups;
using BTIT.EPM.Storage;
using BTIT.EPM.Url;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BTIT.EPM.DigitalSignature
{
    public class DocumentRequestsAppService : EPMAppServiceBase, IDocumentRequestsAppService
    {
        private readonly IRepository<DocumentRequest, long> _documentRequestRepository;
        private readonly IDocumentRequestsExcelExporter _documentRequestsExcelExporter;
        private readonly IRepository<Recipient, long> _recipientsRepository;
        private readonly IRepository<DocumentRequest, long> _lookup_documentRequestRepository;
        private readonly IUserEmailer _userEmailer;
        private readonly UserManager _userManager;
        private readonly IRepository<Recipient, long> _recipientRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IRepository<Document, long> _documentsRepository;
        private readonly IRepository<DocumentRequestAuditTrail, long> _documentRequestAuditTrailRepository;
        private readonly IDocumentRequestAuditTrailsAppService _documentRequestAuditTrailsAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public IAppUrlService AppUrlService { get; set; }
        public DocumentRequestsAppService(IRepository<DocumentRequest, long> documentRequestRepository,
                                          IDocumentRequestsExcelExporter documentRequestsExcelExporter,
                                          IRepository<Recipient, long> recipientRepository,
                                          IBinaryObjectManager binaryObjectManager,
                                          IRepository<Document, long> documentsRepository,
                                          IUserEmailer userEmailer,
                                          IRepository<Recipient, long> recipientsRepository,
                                          IRepository<DocumentRequest, long> lookup_documentRequestRepository,
                                          UserManager userManager,
                                          IRepository<DocumentRequestAuditTrail, long> documentRequestAuditTrailRepository,
                                          IDocumentRequestAuditTrailsAppService documentRequestAuditTrailsAppService,
                                          IAbpSession session,
                                          IEmailSender emailSender,
                                          IHttpContextAccessor httpContextAccessor,
                                          IUnitOfWorkManager unitOfWorkManager)

        {
            _documentRequestRepository = documentRequestRepository;
            _documentRequestsExcelExporter = documentRequestsExcelExporter;
            _documentsRepository = documentsRepository;
            _userEmailer = userEmailer;
            AppUrlService = NullAppUrlService.Instance;
            _lookup_documentRequestRepository = lookup_documentRequestRepository;
            _recipientsRepository = recipientsRepository;
            _recipientRepository = recipientRepository;
            _userManager = userManager;
            _documentRequestAuditTrailRepository = documentRequestAuditTrailRepository;
            _documentRequestAuditTrailsAppService = documentRequestAuditTrailsAppService;
            _binaryObjectManager = binaryObjectManager;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequests)]
        public async Task<PagedResultDto<GetDocumentRequestForViewDto>> GetAll(GetAllDocumentRequestsInput input)
        {
            var statusFilter = (DocumentRequestStatus)input.StatusFilter;

            var filteredDocumentRequests = _documentRequestRepository.GetAll()
                        .Where(e => e.CreatorUserId == AbpSession.UserId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter) || e.MessageBody.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle == input.DocumentTitleFilter)
                        .WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(input.IsSigningOrderedFilter > -1, e => (input.IsSigningOrderedFilter == 1 && e.IsSigningOrdered) || (input.IsSigningOrderedFilter == 0 && !e.IsSigningOrdered));

            var pagedAndFilteredDocumentRequests = filteredDocumentRequests
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var documentRequests = from o in pagedAndFilteredDocumentRequests
                                   join o1 in _documentsRepository.GetAll().Where(x => x.IsActive == true) on o.Id equals o1.DocumentRequestId into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   select new GetDocumentRequestForViewDto()
                                   {
                                       DocumentRequest = new DocumentRequestDto
                                       {
                                           DocumentTitle = string.IsNullOrWhiteSpace(o.DocumentTitle) ? s1.FileName : o.DocumentTitle + s1.FileName,
                                           Status = o.Status,
                                           IsSigningOrdered = o.IsSigningOrdered,
                                           Id = o.Id,
                                           CreationTime = o.CreationTime,
                                           FileGuid = s1.BinaryObjectId == null ? "" : s1.BinaryObjectId.ToString()
                                       }
                                   };
            var documentRequestsDtos = await documentRequests.ToListAsync();

            //File Recepints
            var documentRequestsds = documentRequestsDtos.Select(u => u.DocumentRequest.Id);

            var recipients = await _recipientRepository.GetAll()
                .Where(recipient => documentRequestsds.Contains(recipient.DocumentRequestId.Value))
                .Select(recipient => recipient).ToListAsync();


            foreach (var recipient in recipients)
            {
                documentRequestsDtos.Where(x => x.DocumentRequest.Id == recipient.DocumentRequestId).First().DocumentRequest.Recipients
                    += $"{recipient.FirstName} {recipient.LastName} ({recipient.Email}), ";
            }


            var totalCount = await filteredDocumentRequests.CountAsync();

            return new PagedResultDto<GetDocumentRequestForViewDto>(
                totalCount, documentRequestsDtos
            );
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequests)]
        public async Task<PagedResultDto<GetDocumentRequestForViewDto>> GetAllDocumentRequestWhichNeedToSign(GetAllDocumentRequestsInput input)
        {
            var CurrentLoggedInUserId = AbpSession.UserId;
            var filteredDocumentRequests = _documentRequestRepository.GetAll()
                .Include(x => x.Recipients)
                .Where(e => e.Recipients.Any(s => s.UserId == CurrentLoggedInUserId && !s.IsSigned && s.IsSent && s.IsSigner))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter) || e.MessageBody.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle == input.DocumentTitleFilter)
                .WhereIf(input.IsSigningOrderedFilter > -1, e => (input.IsSigningOrderedFilter == 1 && e.IsSigningOrdered) || (input.IsSigningOrderedFilter == 0 && !e.IsSigningOrdered));

            var pagedAndFilteredDocumentRequests = filteredDocumentRequests
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var documentRequests = from o in pagedAndFilteredDocumentRequests
                                   join o1 in _documentsRepository.GetAll().Where(x => x.IsActive == true) on o.Id equals o1.DocumentRequestId into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   select new GetDocumentRequestForViewDto()
                                   {
                                       DocumentRequest = new DocumentRequestDto
                                       {
                                           DocumentTitle = string.IsNullOrWhiteSpace(o.DocumentTitle) ? s1.FileName : o.DocumentTitle + s1.FileName,
                                           Status = o.Status,
                                           IsSigningOrdered = o.IsSigningOrdered,
                                           Id = o.Id,
                                           CreationTime = o.CreationTime,
                                           FileGuid = s1.BinaryObjectId == null ? "" : s1.BinaryObjectId.ToString()
                                       }
                                   };

            var documentRequestsDtos = await documentRequests.ToListAsync();

            //File Recepints
            var documentRequestsds = documentRequestsDtos.Select(u => u.DocumentRequest.Id);

            var recipients = await _recipientRepository.GetAll()
                .Where(recipient => documentRequestsds.Contains(recipient.DocumentRequestId.Value))
                .Select(recipient => recipient).ToListAsync();


            foreach (var recipient in recipients)
            {
                documentRequestsDtos.Where(x => x.DocumentRequest.Id == recipient.DocumentRequestId).First().DocumentRequest.Recipients
                    += $"{recipient.FirstName} {recipient.LastName} ({recipient.Email}), ";
            }


            var totalCount = await filteredDocumentRequests.CountAsync();
            return new PagedResultDto<GetDocumentRequestForViewDto>(
                totalCount, documentRequestsDtos
                );
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequests)]
        public async Task<int> GetAllDocumentRequestWhichNeedToSignCount()
        {
            var CurrentLoggedInUserId = AbpSession.UserId;
            var filteredDocumentRequests = _documentRequestRepository.GetAll()
                .Include(x => x.Recipients)
                .Where(e => e.Recipients.Any(s => s.UserId == CurrentLoggedInUserId && !s.IsSigned && s.IsSent && s.IsSigner));
            return await filteredDocumentRequests.CountAsync();
        }

        public async Task<GetDocumentRequestForViewDto> GetDocumentRequestForView(long id)
        {
            var documentRequest = await _documentRequestRepository.GetAsync(id);

            var output = new GetDocumentRequestForViewDto { DocumentRequest = ObjectMapper.Map<DocumentRequestDto>(documentRequest) };

            return output;
        }

        [RemoteService(false)]
        public async Task<GetDocumentRequestForViewWithRecipientsDto> GetDocumentRequestForViewWithRecipients(long id)
        {
            var documentRequest = await _documentRequestRepository.GetAsync(id);
            var output = new GetDocumentRequestForViewWithRecipientsDto { DocumentRequest = ObjectMapper.Map<DocumentRequestDto>(documentRequest) };

            output.Recipients = _recipientRepository.GetAll()
               .Where(recipient => recipient.DocumentRequestId == documentRequest.Id)
               .Select(o => new RecipientDto
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
                   Id = o.Id,
                   UserId = o.UserId
               });

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequests_Edit)]
        public async Task<CreateOrEditAllRecipientsDto> GetDocumentRequestForEdit(EntityDto<long> input)
        {
            var documentRequest = await _documentRequestRepository.FirstOrDefaultAsync(input.Id);

            var output = new CreateOrEditAllRecipientsDto
            {
                DocumentRequestId = documentRequest.Id,
                DocumentTitle = documentRequest.DocumentTitle,
                IsSigningOrdered = documentRequest.IsSigningOrdered,
                MessageBody = documentRequest.MessageBody
            };

            var document = _documentsRepository.GetAllList(a => a.DocumentRequestId == documentRequest.Id).FirstOrDefault();

            if (document != null)
            {
                var binaryObject = await _binaryObjectManager.GetOrNullAsync(document.BinaryObjectId.Value);
                output.BinaryObjectId = binaryObject.Id;
                output.BinaryObjectContentType = document.ContentType;
            }

            var reciepints = _recipientRepository.GetAllList(a => a.DocumentRequestId == documentRequest.Id);

            output.Recipients = new List<CreateOrEditRecipientDto>();

            if (reciepints != null && reciepints.Count > 0)
            {
                foreach (var item in reciepints)
                {
                    output.Recipients.Add(new CreateOrEditRecipientDto
                    {
                        Code = item.Code,
                        DocumentRequestId = item.DocumentRequestId,
                        Email = item.Email,
                        FieldName = item.FieldName,
                        FirstName = item.FirstName,
                        Id = item.Id,
                        IsSigned = item.IsSigned,
                        IsSigner = item.IsSigner,
                        LastName = item.LastName,
                        SignatureDate = item.SignatureDate,
                        SigneOrder = item.SigneOrder,
                        SignerPin = item.SignerPin,
                        Type = item.Type,
                        UserId = item.UserId,
                        ViewDate = item.ViewDate,
                        MobileNumber = item.MobileNumber
                    });
                }
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequests_Create)]
        [UnitOfWork]
        public async Task SaveOrSend(CreateOrEditAllRecipientsDto input, bool IsSend)
        {
            if (input.Recipients == null || input.Recipients.Count == 0)
            {
                throw new UserFriendlyException(L("AtleastOneRecipient"));
            }
            //======================== Document Request ================================
            var documentRequest = await _documentRequestRepository.FirstOrDefaultAsync((long)input.DocumentRequestId);
            var document = await _documentsRepository.FirstOrDefaultAsync(x => x.DocumentRequestId == documentRequest.Id);
            documentRequest.IsSigningOrdered = input.IsSigningOrdered;
            documentRequest.DocumentTitle = input.DocumentTitle;
            documentRequest.MessageBody = input.MessageBody;

            //===========================================================================


            //======================= Recipients ========================================

            //===== Old =========
            var oldRecipients = _recipientRepository.GetAllList(a => a.DocumentRequestId == input.DocumentRequestId);
            if (oldRecipients != null && oldRecipients.Count > 0)
            {
                foreach (var item in oldRecipients)
                {
                    await _recipientRepository.DeleteAsync(item);
                }
            }
            //===================
            List<Recipient> recipients = new List<Recipient>();

            //===== New =========
            foreach (var recipientInput in input.Recipients)
            {
                if (recipientInput.Type == RecipientType.Me)
                {
                    recipientInput.UserId = AbpSession.UserId;
                }
                else if (recipientInput.Type == RecipientType.External)
                {
                    recipientInput.UserId = null;
                }

                if (string.IsNullOrWhiteSpace(recipientInput.FieldName))
                {
                    recipientInput.IsSigner = false;
                }
                else
                {
                    recipientInput.IsSigner = true;
                }

                recipientInput.DocumentRequestId = input.DocumentRequestId;
                recipientInput.Code = Guid.NewGuid();
                if (recipientInput.UserId.HasValue)
                {
                    var receiver = new UserIdentifier(AbpSession.TenantId, recipientInput.UserId.Value);
                    var user = _userManager.GetUser(receiver);
                    recipientInput.Email = user.EmailAddress;
                    recipientInput.FirstName = user.Name;
                    recipientInput.LastName = user.Surname;
                }

                var recipient = new Recipient
                {
                    Code = recipientInput.Code.Value,
                    DocumentRequestId = recipientInput.DocumentRequestId,
                    Email = recipientInput.Email,
                    FieldName = recipientInput.FieldName,
                    FirstName = recipientInput.FirstName,
                    IsSigned = recipientInput.IsSigned,
                    IsSigner = recipientInput.IsSigner,
                    LastName = recipientInput.LastName,
                    SignatureDate = recipientInput.SignatureDate,
                    SigneOrder = recipientInput.SigneOrder,
                    SignerPin = recipientInput.SignerPin,
                    Type = recipientInput.Type,
                    UserId = recipientInput.UserId,
                    ViewDate = recipientInput.ViewDate,
                    MobileNumber = recipientInput.MobileNumber,
                    SignerPinTriesCount = 0

                };
                await _recipientRepository.InsertAsync(recipient);
                recipients.Add(recipient);

            }
            //===================
            await CurrentUnitOfWork.SaveChangesAsync();
            if (IsSend)
            {
                foreach (var item in recipients)
                {
                    await _userEmailer.SendEmailViewAndSignDocumentAsync(AbpSession.TenantId.Value, item.FirstName, item.LastName
                    , item.Email, item.Id, documentRequest.DocumentTitle + " - " + document.FileName
                    , AppUrlService.CreateViewAndSignDocumentAsyncUrlFormat(item.Type == RecipientType.External, AbpSession.TenantId)
                    , documentRequest.Id, item.IsSigner, AuditTrailType.Sent, item.Code.ToString());

                    item.IsSent = true;
                    item.SentDate = DateTime.Now;

                    await _documentRequestAuditTrailsAppService.AddAuditByDocumentRequestIdAsync(documentRequest.Id, AuditTrailType.Sent, recipientId: item.Id);

                }
                documentRequest.Status = DocumentRequestStatus.InProgress;
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            //===========================================================================
        }

        public async Task<long> CreateOrEdit(CreateOrEditDocumentRequestDto input)
        {
            long id;
            if (input.Id == null)
            {
                id = await Create(input);
            }
            else
            {
                id = await Update(input);
            }
            return id;
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequests_Create)]
        protected virtual async Task<long> Create(CreateOrEditDocumentRequestDto input)
        {
            var documentRequest = ObjectMapper.Map<DocumentRequest>(input);


            if (AbpSession.TenantId != null)
            {
                documentRequest.TenantId = (int)AbpSession.TenantId;
            }


            var id = await _documentRequestRepository.InsertAndGetIdAsync(documentRequest);
            return id;
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequests_Edit)]
        protected virtual async Task<long> Update(CreateOrEditDocumentRequestDto input)
        {
            var documentRequest = await _documentRequestRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, documentRequest);
            return documentRequest.Id;
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequests_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _documentRequestRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequests)]
        public async Task<FileDto> GetDocumentRequestsToExcel(GetAllDocumentRequestsForExcelInput input)
        {
            var statusFilter = (DocumentRequestStatus)input.StatusFilter;

            var filteredDocumentRequests = _documentRequestRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter) || e.MessageBody.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle == input.DocumentTitleFilter)
                        .WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(input.IsSigningOrderedFilter > -1, e => (input.IsSigningOrderedFilter == 1 && e.IsSigningOrdered) || (input.IsSigningOrderedFilter == 0 && !e.IsSigningOrdered));

            var query = (from o in filteredDocumentRequests
                         select new GetDocumentRequestForViewDto()
                         {
                             DocumentRequest = new DocumentRequestDto
                             {
                                 DocumentTitle = o.DocumentTitle,
                                 Status = o.Status,
                                 IsSigningOrdered = o.IsSigningOrdered,
                                 Id = o.Id
                             }
                         });


            var documentRequestListDtos = await query.ToListAsync();

            return _documentRequestsExcelExporter.ExportToFile(documentRequestListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequests)]
        public async Task SendEmailViewAndSignDocument(int tenantId, string firstName, string lastName, string email,
            long recipientId, string documentTitle, long documentRequestId, bool isSignRequired
            , string recipientCode, bool pinUrl, int auditTrailType)
        {

            await _userEmailer.SendEmailViewAndSignDocumentAsync(
                         tenantId, firstName, lastName, email,
             recipientId, documentTitle, AppUrlService.CreateViewAndSignDocumentAsyncUrlFormat(pinUrl, AbpSession.TenantId), documentRequestId, isSignRequired
            , (AuditTrailType)auditTrailType, recipientCode);
        }

        [AllowAnonymous]
        public async Task<PagedResultDto<GetDocumentRequestViewAndSignDto>> GetDocumentRequestForViewAndSign(ViewAndSignDocumentEmailInput input)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                httpContext.Items["__ViewAndSignDocumentEmailInput"] = input;
                var cachedValue = httpContext.Items["__ViewAndSignDocumentEmailInput"] as ViewAndSignDocumentEmailInput;

                var documentRequest = _recipientsRepository.GetAll()
                    .Include(x => x.DocumentRequestFk)
                    .Where(x => x.DocumentRequestId == input.DocumentRequestId && x.IsSigner == true);

                var pagedAndDocumentRequest = documentRequest.OrderBy("id asc");

                var documentRequestViewAndSign = from o in pagedAndDocumentRequest
                                                 select new GetDocumentRequestViewAndSignDto()
                                                 {
                                                     documentRequestViewAndSign = new DocumentRequestViewAndSignDto
                                                     {
                                                         Code = o.Code,
                                                         DocumentRequestId = o.DocumentRequestId,
                                                         Email = o.Email,
                                                         IsSigned = o.IsSigned,
                                                         Name = o.FirstName + " " + o.LastName,
                                                         IsSigner = o.IsSigner,
                                                         SignatureDate = o.SignatureDate,
                                                         SigneOrder = o.SigneOrder,
                                                         SignerPin = o.SignerPin,
                                                         UserId = o.UserId,
                                                         ViewDate = o.ViewDate,
                                                         SentDate = o.SentDate,
                                                         IsSent = o.IsSent,
                                                         DocumentRequest = new DocumentRequestDto
                                                         {
                                                             DocumentTitle = o.DocumentRequestFk.DocumentTitle,
                                                             Status = o.DocumentRequestFk.Status,
                                                             IsSigningOrdered = o.DocumentRequestFk.IsSigningOrdered,
                                                             Id = o.DocumentRequestFk.Id,
                                                         }
                                                     }
                                                 };

                var totalCount = await documentRequestViewAndSign.CountAsync();

                return new PagedResultDto<GetDocumentRequestViewAndSignDto>(
                    totalCount,
                    await documentRequestViewAndSign.ToListAsync()
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AllowAnonymous]
        [RemoteService(false)]
        public async Task<GetDocumentRequestAuditTrailsForViewDto> GetDocumentRequestAuditTrail(long id)
        {

            var documentRequest = await _lookup_documentRequestRepository.GetAll()
                        .Include(e => e.Documents)
                        .Where(x => x.Id == id).FirstAsync();

            var documentRequestAuditTrails = _documentRequestAuditTrailRepository.GetAll()
                        .Include(e => e.RecipientFk)
                        .Where(x => x.DocumentRequestId == id).OrderBy(x => x.CreationTime);

            var documentRequestAuditTrailForViewDto = new GetDocumentRequestAuditTrailsForViewDto();

            documentRequestAuditTrailForViewDto.DocumentRequestDocumentTitle = documentRequest.DocumentTitle;
            documentRequestAuditTrailForViewDto.DocumentId = documentRequest.Documents.First(x => x.IsActive == true).BinaryObjectId.Value;
            documentRequestAuditTrailForViewDto.FileName = documentRequest.Documents.First(x => x.IsActive == true).FileName;
            documentRequestAuditTrailForViewDto.Status = documentRequest.Status;
            documentRequestAuditTrailForViewDto.DocumentRequestAuditTrailForViewDetailsDto =
                documentRequestAuditTrails.Select(x => new GetDocumentRequestAuditTrailForViewDetailsDto()
                {
                    ClientIpAddress = x.ClientIpAddress,
                    CreationTime = x.CreationTime,
                    RecipientEmail = x.RecipientFk.Email,
                    RecipientName = $"{x.RecipientFk.FirstName} {x.RecipientFk.LastName}",
                    Type = x.Type
                }).ToList();

            if (documentRequestAuditTrailForViewDto.DocumentRequestAuditTrailForViewDetailsDto.Any(x => x.Type == AuditTrailType.Sent))
            {
                string SentRecipient = string.Empty;
                foreach (var item in documentRequestAuditTrailForViewDto.DocumentRequestAuditTrailForViewDetailsDto.Where(x => x.Type == AuditTrailType.Sent))
                {
                    SentRecipient += $"{item.RecipientName}({item.RecipientEmail}), ";
                }
                documentRequestAuditTrailForViewDto.DocumentRequestAuditTrailForViewDetailsDto.First(x => x.Type == AuditTrailType.Sent).Message = SentRecipient;
                documentRequestAuditTrailForViewDto.DocumentRequestAuditTrailForViewDetailsDto.RemoveAll(x => x.Type == AuditTrailType.Sent && string.IsNullOrEmpty(x.Message));
            }
            return documentRequestAuditTrailForViewDto;
        }

        [AllowAnonymous]
        [RemoteService(false)]
        public async Task UpdateDocument(long id, string Code, byte[] Signature, string PinCode = null)
        {
            var tenantId = AbpSession.TenantId;
            if (!tenantId.HasValue)
                tenantId = 1;
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var recipient = await _recipientsRepository.GetAsync(id);

                if (Code != recipient.Code.ToString())
                    throw new UserFriendlyException(L("Unauthorized"));

                if (recipient.Type == RecipientType.External && recipient.SignerPin != PinCode)
                {
                    throw new UserFriendlyException(L("IncorrectPin"));
                }

                else if (recipient.Type != RecipientType.External)
                {
                    if (recipient.UserId != AbpSession.UserId)
                        throw new UserFriendlyException(L("Unauthorized User"));
                }
                var doc = await _documentsRepository.FirstOrDefaultAsync(e => e.DocumentRequestId == recipient.DocumentRequestId && e.IsActive == true);

                var fileObject = await _binaryObjectManager.GetOrNullAsync(doc.BinaryObjectId.Value);
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(fileObject.Bytes);

                PdfLoadedPage page = loadedDocument.Pages[0] as PdfLoadedPage;

                PdfLoadedSignatureField signatureField = loadedDocument.Form.Fields[recipient.FieldName] as PdfLoadedSignatureField;

                FileStream certificateStream1 = new FileStream("PDF.pfx", FileMode.Open, FileAccess.Read);
                PdfCertificate certificate1 = new PdfCertificate(certificateStream1, "syncfusion");

                signatureField.Signature = new PdfSignature(loadedDocument, page, certificate1, "Signature", signatureField);
                //FileStream imageStream = new FileStream("ClientSignature.png", FileMode.Open, FileAccess.Read);

                MemoryStream stream = new MemoryStream();
                stream.Write(Signature, 0, Signature.Length);
                stream.Position = 0;

                PdfBitmap signatureImage = new PdfBitmap(stream);
                signatureField.Signature.Appearance.Normal.Graphics.DrawImage(signatureImage, 0, 0, 90, 20);

                MemoryStream signedStream = new MemoryStream();
                loadedDocument.Save(signedStream);
                signedStream.Position = 0;

                var fileBytes = signedStream.ToArray();

                var newFileObject = new BinaryObject(tenantId, fileBytes);

                //================== Save Binary and Get ID =============================
                var objectBinaryId = await _binaryObjectManager.SaveAndGetIdAsync(newFileObject);
                //doc.IsActive = false;
                doc.BinaryObjectId = newFileObject.Id;

                recipient.IsSigned = true;
                recipient.SignatureDate = DateTime.Now;



                await _documentRequestAuditTrailRepository.InsertAndGetIdAsync(new DocumentRequestAuditTrail
                {
                    DocumentRequestId = recipient.DocumentRequestId.Value,
                    RecipientId = recipient.Id,
                    Type = AuditTrailType.Signed
                });

                var unSignedCount = _recipientsRepository.GetAll().Where(r => r.DocumentRequestId == recipient.DocumentRequestId && r.Id != recipient.Id && r.IsSigned == false).Count();
                if (unSignedCount == 0)
                {
                    await _documentRequestAuditTrailRepository.InsertAndGetIdAsync(new DocumentRequestAuditTrail
                    {
                        DocumentRequestId = recipient.DocumentRequestId.Value,
                        Type = AuditTrailType.Completed
                    });
                    var request = await _documentRequestRepository.GetAsync(recipient.DocumentRequestId.Value);
                    request.Status = DocumentRequestStatus.Completed;
                }
            }
        }


        [AllowAnonymous]
        [RemoteService(false)]
        public async Task<bool> GeneratePin(ViewAndSignDocumentEmailInput input)
        {

            var recipient = await _recipientsRepository.GetAll().Where(x => x.DocumentRequestId == input.DocumentRequestId && x.Id == input.RecipientId && x.Code == new Guid(input.RecipientCode)).FirstOrDefaultAsync();
            if (recipient.Type == RecipientType.External)
            {
                recipient.SignerPin = new Random().Next(10005, 99997).ToString();
                recipient.SignerPinExpiry = DateTime.Now.AddHours(1);
                await _emailSender.SendAsync(new MailMessage
                {
                    To = { recipient.Email },
                    Subject = "UseThisPinCode",
                    Body = recipient.SignerPin,
                    IsBodyHtml = false
                });
                return true;
            }

            return false;
        }

        [AllowAnonymous]
        public async Task<bool> ConfirmPin(ViewAndSignDocumentEmailInput input)
        {
            var recipient = await _recipientsRepository.GetAll().Where(x => x.DocumentRequestId == input.DocumentRequestId && x.Id == input.RecipientId && x.Type == RecipientType.External && x.Code == new Guid(input.RecipientCode)).FirstOrDefaultAsync();

            if (recipient.SignerPinTriesCount > 5)
                throw new UserFriendlyException(L("SignerPinTriesCountExcceded"));

            var _signerPin = recipient.SignerPin;

            if (_signerPin != input.SignerPin)
            {
                recipient.SignerPinTriesCount += 1;
                return false;
            }
            else return true;
        }


        [AllowAnonymous]
        [RemoteService(false)]
        public bool CheckPin(long recipientId, string PinCode)
        {
            var recipient = _recipientsRepository.Get(recipientId);

            if (recipient.Type != RecipientType.External)
                return true;

            if (recipient.SignerPinTriesCount > 5)
                throw new UserFriendlyException(L("SignerPinTriesCountExcceded"));

            var _signerPin = recipient.SignerPin;

            if (_signerPin != PinCode)
            {
                recipient.SignerPinTriesCount += 1;
                return false;
            }
            else return true;
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequests)]
        public async Task SendReminderEmail(int DocumentRequestID)
        {
            var recipient = await _recipientsRepository.GetAll()
                .Include(e => e.DocumentRequestFk)
                .ThenInclude(e => e.Documents)
                .Where(e => e.DocumentRequestId == DocumentRequestID)
                .Where(e => e.IsSent && e.IsSigner && !e.IsSigned).ToListAsync();
            foreach (var item in recipient)
            {
                item.SignerPinTriesCount = 0;
                await _userEmailer.SendEmailViewAndSignDocumentAsync(AbpSession.TenantId.Value, item.FirstName, item.LastName
                        , item.Email, item.Id, item.DocumentRequestFk.DocumentTitle + " - " + item.DocumentRequestFk.Documents.FirstOrDefault().FileName
                        , AppUrlService.CreateViewAndSignDocumentAsyncUrlFormat(item.Type == RecipientType.External, AbpSession.TenantId)
                        , item.DocumentRequestFk.Id, item.IsSigner, AuditTrailType.Sent, item.Code.ToString());
            }
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentRequests)]
        public async Task<string> GetDocumentLink(int DocumentRequestID)
        {
            var recipient = await _recipientsRepository.GetAll()
                .Include(e => e.DocumentRequestFk)
                .ThenInclude(e => e.Documents)
                .Where(e => e.DocumentRequestId == DocumentRequestID && e.UserId == AbpSession.UserId ).FirstOrDefaultAsync();

            var link = AppUrlService.CreateViewAndSignDocumentAsyncUrlFormat(recipient.Type == RecipientType.External, AbpSession.TenantId);

            link = link.Replace("{recipientId}", recipient.Id.ToString());
            link = link.Replace("{documentRequestId}", recipient.DocumentRequestId.ToString());
            link = link.Replace("{recipientCode}", Uri.EscapeDataString(recipient.Code.ToString()));
            link = link.Replace("{tenantId}", AbpSession.TenantId.ToString());


            return _userEmailer.EncryptQueryParameters(link);
        }
    }
}