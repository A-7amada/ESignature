
using BTIT.EPM.Models;
using Microsoft.AspNetCore.Html;
using System.Text;

namespace BTIT.EPM.Utils
{
    public class GcPdfViewerRenderHelper
    {

        public static IHtmlContent RenderOptions(PdfViewerOptions options, string instanceName = "viewer")
        {
            StringBuilder sb = new StringBuilder();
            if (options.ViewerTools != null)
            {
                sb.AppendLine($"{instanceName}.toolbarLayout.viewer.default = ['{string.Join("', '", options.ViewerTools)}'];");
            }
            if (options.AnnotationEditorTools != null)
            {
                sb.AppendLine($"{instanceName}.toolbarLayout.annotationEditor.default = ['{string.Join("', '", options.AnnotationEditorTools)}'];");
            }
            if (options.FormEditorTools != null)
            {
                sb.AppendLine($"{instanceName}.toolbarLayout.formEditor.default = ['{string.Join("', '", options.FormEditorTools)}'];");
            }
            if (sb.Length > 0)
            {
                sb.AppendLine($"{instanceName}.applyToolbarLayout();");
            }
            if ((options.ViewerOptions & PdfViewerOptions.Options.AnnotationEditorPanel) != 0)
            {
                sb.AppendLine($"{instanceName}.addAnnotationEditorPanel();");
                if ((options.ViewerOptions & PdfViewerOptions.Options.ActivateAnnotationEditor) != 0)
                {
                    sb.AppendLine($"{instanceName}.layoutMode = 1;");
                }
            }
            if ((options.ViewerOptions & PdfViewerOptions.Options.FormEditorPanel) != 0)
            {
                sb.AppendLine($"{instanceName}.addFormEditorPanel();");
                if ((options.ViewerOptions & PdfViewerOptions.Options.FormEditorPanel) != 0)
                {
                    sb.AppendLine($"{instanceName}.layoutMode = 2;");
                }
            }
            if ((options.ViewerOptions & PdfViewerOptions.Options.ExpandedReplyTool) != 0)
            {
                sb.AppendLine($"{instanceName}.addReplyTool('expanded');");
            }
            else if ((options.ViewerOptions & PdfViewerOptions.Options.ReplyTool) != 0)
            {
                sb.AppendLine($"{instanceName}.addReplyTool();");
            }
            return new HtmlString(sb.ToString());
        }
    }
}
