using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using BTIT.EPM.Authorization;
using BTIT.EPM.Storage;
using BTIT.EPM.Web.Controllers;
using System.IO;
using Microsoft.AspNetCore.Http;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Security;
using Syncfusion.Pdf.Graphics;
using System.Collections.Generic;

namespace BTIT.EPM.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_DemoUiComponents)]
    public class DemoUiComponentsController : EPMControllerBase
    {
        private readonly IBinaryObjectManager _binaryObjectManager;

        public DemoUiComponentsController(IBinaryObjectManager binaryObjectManager)
        {
            _binaryObjectManager = binaryObjectManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> UploadFile(string defaultFileUploadTextInput)
        {
            try
            {
                var file = Request.Form.Files.First();

                //Check input
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576) //1MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    PdfLoadedDocument loadedDocument = new PdfLoadedDocument(stream);

                    //Gets the first page of the document
                    PdfLoadedPage page = loadedDocument.Pages[0] as PdfLoadedPage;

                    List<PdfLoadedSignatureField> DocumentSignatureFields = new List<PdfLoadedSignatureField>();

                    foreach (var item in loadedDocument.Form.Fields)
                    {
                        if (item is PdfLoadedSignatureField)
                            DocumentSignatureFields.Add(item as PdfLoadedSignatureField);
                    }
                    if (DocumentSignatureFields.Count == 0)
                        throw new UserFriendlyException(L("No_SignatureFields_Found_Error"));
                    fileBytes = stream.GetAllBytes();
                }

                var fileObject = new BinaryObject(AbpSession.TenantId, fileBytes);
                await _binaryObjectManager.SaveAsync(fileObject);

                return Json(new AjaxResponse(new
                {
                    id = fileObject.Id,
                    contentType = file.ContentType,
                    defaultFileUploadTextInput = string.IsNullOrEmpty(defaultFileUploadTextInput) ? file.FileName : defaultFileUploadTextInput
                }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        public async Task<ActionResult> GetFile(Guid id, string contentType)
        {
            var fileObject = await _binaryObjectManager.GetOrNullAsync(id);
            if (fileObject == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            return File(fileObject.Bytes, contentType);
        }

        public IActionResult GeneratePDF(Stream docStream)
        {
            //Load the PDF document
            //FileStream docStream = new FileStream(HttpContext.Session.GetString("filepath"), FileMode.Open, FileAccess.Read);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(docStream);

            //Gets the first page of the document
            PdfLoadedPage page = loadedDocument.Pages[0] as PdfLoadedPage;

            List<PdfLoadedSignatureField> DocumentSignatureFields = new List<PdfLoadedSignatureField>();

            foreach (var item in loadedDocument.Form.Fields)
            {
                if (item is PdfLoadedSignatureField)
                    DocumentSignatureFields.Add(item as PdfLoadedSignatureField);
            }
            if (DocumentSignatureFields.Count == 0)
                throw new UserFriendlyException(L("No_SignatureFields_Found_Error"));

            //Gets the first signature field of the PDF document
            PdfLoadedSignatureField signatureField1 = loadedDocument.Form.Fields["ClientSignature"] as PdfLoadedSignatureField;

            //Creates a certificate
            FileStream certificateStream1 = new FileStream("PDF.pfx", FileMode.Open, FileAccess.Read);
            PdfCertificate certificate1 = new PdfCertificate(certificateStream1, "syncfusion");

            signatureField1.Signature = new PdfSignature(loadedDocument, page, certificate1, "Signature", signatureField1);
            FileStream imageStream = new FileStream("ClientSignature.png", FileMode.Open, FileAccess.Read);
            PdfBitmap signatureImage = new PdfBitmap(imageStream);
            signatureField1.Signature.Appearance.Normal.Graphics.DrawImage(signatureImage, 0, 0, 90, 20);

            //Save the document into stream
            MemoryStream stream = new MemoryStream();
            loadedDocument.Save(stream);

            //Load the signed PDF document
            PdfLoadedDocument signedDocument = new PdfLoadedDocument(stream);

            //Load the PDF page
            PdfLoadedPage loadedPage = signedDocument.Pages[0] as PdfLoadedPage;

            //Gets the first signature field of the PDF document
            PdfLoadedSignatureField signatureField2 = signedDocument.Form.Fields["VendorSignature"] as PdfLoadedSignatureField;

            signatureField1.Signature = new PdfSignature(signedDocument, loadedPage, certificate1, "Signature", signatureField2);
            FileStream imageStream1 = new FileStream("VendorSignature.jpeg", FileMode.Open, FileAccess.Read);
            PdfBitmap signatureImage1 = new PdfBitmap(imageStream1);
            signatureField1.Signature.Appearance.Normal.Graphics.DrawImage(signatureImage1, 0, 0, 90, 20);

            //Saving the PDF to the MemoryStream
            MemoryStream signedStream = new MemoryStream();

            signedDocument.Save(signedStream);

            //Set the position as '0'.
            signedStream.Position = 0;

            //Download the PDF document in the browser
            FileStreamResult fileStreamResult = new FileStreamResult(signedStream, "application/pdf");

            fileStreamResult.FileDownloadName = "DigitalSignatureSample.pdf";

            return fileStreamResult;
        }
    }
}