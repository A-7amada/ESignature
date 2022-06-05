using System;
using System.Reflection;
using System.IO;
using System.Linq;
using GrapeCity.Documents.Pdf.ViewerSupportApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using GrapeCity.Documents.Pdf.ViewerSupportApi.Utils;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using GrapeCity.Documents.Pdf;
using Abp.Authorization;
using BTIT.EPM.Sampels;


namespace BTIT.EPM.Web.Controllers
{
    [Route("api/pdf-viewer")]
    [ApiController]
    [AbpAllowAnonymous]
    public class SampleSupportController : GcPdfViewerController
    {
        static SampleSupportController()
        {
#if DEBUG
            // VS's F5 sets the current working directory to project dir rather than bin dir,
            // we set it to bin dir so that we can fetch files from the Resources/ sub-dir:
            var exePath = new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath;
            var directory = Path.GetDirectoryName(exePath);
            Directory.SetCurrentDirectory(directory);
#endif
            Settings.AvailableUsers.AddRange(new string[] { "James", "Susan Smith" });
        }

        /// <summary>
        /// Generate the sample PDF.
        /// </summary>
        /// <returns></returns>
        [Route("get-sample-pdf")]
        public virtual IActionResult GetSamplePdf()
        {
            Response.Headers["Content-Disposition"] = $"inline; filename=\"GcPdfViewerDemo.pdf\"";
            var result = new FileStreamResult(SamplePdfCreator.CreatePdf(), "application/pdf");
            return result;
        }

        /// <summary>
        /// The method receives requests from the document list panel,
        /// see DocumentList below.
        /// </summary>
        /// <returns></returns>
        [Route("get-pdf-from-list")]
        public virtual IActionResult GetPdfFromList(string name)
        {
            Response.Headers["Content-Disposition"] = $"inline; filename=\"{name}\"";
            var filePath = Path.Combine("Resources", "PDFs", name);
            return new FileStreamResult(System.IO.File.OpenRead(filePath), "application/pdf");
        }

        /// <summary>
        /// This method is used by the Document List Panel sample.
        /// </summary>
        /// <returns></returns>
        [Route("DocumentList")]
        public object DocumentList()
        {
            var directoryInfo = new DirectoryInfo(Path.Combine("Resources", "PDFs"));
            var allPdfs = directoryInfo.GetFiles("*.pdf");
            return _PrepareJsonAnswer(allPdfs.Select(
                f_ => $"{f_.Name}|api/pdf-viewer/get-pdf-from-list?name={f_.Name}|Open {f_.Name}"));
        }

        /// <summary>
        /// As an example, override one of the base Support API methods.
        /// </summary>
        /// <returns></returns>
        public override string Ping(string docId)
        {
            return base.Ping(docId);
        }

        public override void OnDocumentModified(GcPdfDocumentLoader documentLoader)
        {
            CleanupSampleCloudStorage();
            var userData = documentLoader.Info.documentOptions.userData as JObject;
            if (userData != null)
            {
                var userDataObj = userData.ToObject<Dictionary<string, string>>();
                if (userDataObj != null)
                {
                    if (userDataObj.ContainsKey("sampleName") && userDataObj["sampleName"] == "SaveChangesSample")
                    {
                        string docName = userDataObj["docName"];
                        SaveDocumentToCloud(documentLoader.ClientId, documentLoader.Document, docName);
                    }

                }
            }
        }

        #region ** "Custom save button" sample:

        public static Dictionary<string, KeyValuePair<DateTime, byte[]>> DocumentsInCloud { get; private set; } = new Dictionary<string, KeyValuePair<DateTime, byte[]>>();

        [Route("GetPdfFromCloud")]
        public IActionResult GetPdfFromCloud(string docName, string clientId)
        {
            var fileBytes = GetDocumentFromCloud(docName, clientId);
            if (fileBytes == null)
                throw new Exception($"Sample document '{docName}' not found.");
            return new FileContentResult(fileBytes, "application/pdf")
            {
                FileDownloadName = docName
            };
        }

        public void SaveDocumentToCloud(string clientId, GcPdfDocument document, string docName)
        {
            var key = $"{docName}_{clientId}";
            MemoryStream ms = new MemoryStream();
            document.Save(ms);
            lock (DocumentsInCloud)
            {
                if (DocumentsInCloud.ContainsKey(key))
                {
                    DocumentsInCloud.Remove(key);
                }
                DocumentsInCloud.Add(key, new KeyValuePair<DateTime, byte[]>(DateTime.Now, ms.ToArray()));
            }
            ms.Dispose();
        }

        public static byte[] GetDocumentFromCloud(string docName, string clientId)
        {
            var key = $"{docName}_{clientId}";
            byte[] bytes = null;
            lock (DocumentsInCloud)
            {
                bytes = DocumentsInCloud.ContainsKey(key) ? DocumentsInCloud[key].Value : null;
            }
            CleanupSampleCloudStorage();
            return bytes;

        }

        static void CleanupSampleCloudStorage()
        {
            lock (DocumentsInCloud)
            {
                foreach (var k in DocumentsInCloud.Keys)
                {
                    if ((DateTime.Now - DocumentsInCloud[k].Key) > new TimeSpan(0, 10, 0) /* 10 min */)
                    {
                        DocumentsInCloud.Remove(k);
                    }
                }
            }
        }

        #endregion

    }
}
