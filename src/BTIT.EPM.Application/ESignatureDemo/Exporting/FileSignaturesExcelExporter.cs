using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BTIT.EPM.DataExporting.Excel.NPOI;
using BTIT.EPM.ESignatureDemo.Dtos;
using BTIT.EPM.Dto;
using BTIT.EPM.Storage;

namespace BTIT.EPM.ESignatureDemo.Exporting
{
    public class FileSignaturesExcelExporter : NpoiExcelExporterBase, IFileSignaturesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FileSignaturesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFileSignatureForViewDto> fileSignatures)
        {
            return CreateExcelPackage(
                "FileSignatures.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("FileSignatures"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Describtion")
                        );

                    AddObjects(
                        sheet, 2, fileSignatures,
                        _ => _.FileSignature.Name,
                        _ => _.FileSignature.Describtion
                        );

                });
        }
    }
}