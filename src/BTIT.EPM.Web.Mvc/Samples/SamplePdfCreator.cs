using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using GrapeCity.Documents.Pdf;
using GrapeCity.Documents.Text;

namespace BTIT.EPM.Sampels
{
    public static class SamplePdfCreator
    {
        public static Stream CreatePdf()
        {
            var stream = GetPdfStream();
            if (stream != null)
                return stream;

            return CreatePdf("GcPdfViewer sample PDF.");
        }

        public static Stream CreatePdf(string text)
        {
            var stream = new MemoryStream();
            GcPdfDocument doc = new GcPdfDocument();
            GcPdfGraphics g = doc.NewPage().Graphics;
            g.DrawString(text, new TextFormat() { Font = StandardFonts.Times, FontSize = 12 }, new PointF(72, 72));
            doc.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static Models.PdfViewerOptions GetViewerOptions()
        {
            Type sampleType = GetSampleType(out MethodInfo unused);
            if (sampleType == null)
                return new Models.PdfViewerOptions();

            var viewerOptionsProp = sampleType.GetTypeInfo().GetProperty("PdfViewerOptions", BindingFlags.Static | BindingFlags.Public);
            if (viewerOptionsProp == null)
                return new Models.PdfViewerOptions();

            return viewerOptionsProp.GetValue(null) as Models.PdfViewerOptions;
        }

        private static Stream GetPdfStream()
        {
            Type sampleType = GetSampleType(out MethodInfo createPdf);
            if (sampleType == null)
                return null;

            var sample = Activator.CreateInstance(sampleType);
            MemoryStream ms = new MemoryStream();
            var ret = createPdf.Invoke(sample, new object[] { ms });
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        private static Type GetSampleType(out MethodInfo createPdf)
        {
            const string createPdfName = "CreatePDF";
            Type[] createPdfArgs = new Type[] { typeof(Stream) };

            var asm = Assembly.GetExecutingAssembly();
            createPdf = null;
            foreach (var t in asm.GetExportedTypes())
            {
                createPdf = t.GetTypeInfo().GetMethod(createPdfName, createPdfArgs);
                if (createPdf != null)
                    return t;
            }
            return null;
        }
    }
}
