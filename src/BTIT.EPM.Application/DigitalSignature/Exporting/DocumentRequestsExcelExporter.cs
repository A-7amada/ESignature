using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BTIT.EPM.DataExporting.Excel.EpPlus;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Dto;
using BTIT.EPM.Storage;

namespace BTIT.EPM.DigitalSignature.Exporting
{
    public class DocumentRequestsExcelExporter : EpPlusExcelExporterBase, IDocumentRequestsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DocumentRequestsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDocumentRequestForViewDto> documentRequests)
        {
            return CreateExcelPackage(
                "DocumentRequests.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("DocumentRequests"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DocumentTitle"),
                        L("Status"),
                        L("IsSigningOrdered")
                        );

                    AddObjects(
                        sheet, 2, documentRequests,
                        _ => _.DocumentRequest.DocumentTitle,
                        _ => _.DocumentRequest.Status,
                        _ => _.DocumentRequest.IsSigningOrdered
                        );

					

                });
        }
    }
}
