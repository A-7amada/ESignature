using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BTIT.EPM.DataExporting.Excel.EpPlus;
using BTIT.EPM.Documents.Dtos;
using BTIT.EPM.Dto;
using BTIT.EPM.Storage;

namespace BTIT.EPM.Documents.Exporting
{
    public class DocumentsExcelExporter : EpPlusExcelExporterBase, IDocumentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DocumentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDocumentForViewDto> documents)
        {
            return CreateExcelPackage(
                "Documents.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Documents"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("FileName"),
                        L("Extension"),
                        L("Size"),
                        L("ContentType"),
                        //L("IsActive"),
                        (L("BinaryObject")) + L("TenantId"),
                        (L("DocumentRequest")) + L("DocumentTitle")
                        );

                    AddObjects(
                        sheet, 2, documents,
                        _ => _.Document.FileName,
                        _ => _.Document.Extension,
                        _ => _.Document.Size,
                        _ => _.Document.ContentType,
                        //_ => _.Document.IsActive,
                        _ => _.BinaryObjectTenantId,
                        _ => _.DocumentRequestDocumentTitle
                        );

					

                });
        }
    }
}
