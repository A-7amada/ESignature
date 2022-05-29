using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BTIT.EPM.DataExporting.Excel.EpPlus;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Dto;
using BTIT.EPM.Storage;

namespace BTIT.EPM.DigitalSignature.Exporting
{
    public class DocumentRequestAuditTrailsExcelExporter : EpPlusExcelExporterBase, IDocumentRequestAuditTrailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DocumentRequestAuditTrailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDocumentRequestAuditTrailForViewDto> documentRequestAuditTrails)
        {
            return CreateExcelPackage(
                "DocumentRequestAuditTrails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("DocumentRequestAuditTrails"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Type"),
                        L("ClientIpAddress"),
                        (L("DocumentRequest")) + L("DocumentTitle"),
                        (L("Recipient")) + L("FirstName")
                        );

                    AddObjects(
                        sheet, 2, documentRequestAuditTrails,
                        _ => _.DocumentRequestAuditTrail.Type,
                        _ => _.DocumentRequestAuditTrail.ClientIpAddress,
                        _ => _.DocumentRequestDocumentTitle,
                        _ => _.RecipientFirstName
                        );

					

                });
        }
    }
}
