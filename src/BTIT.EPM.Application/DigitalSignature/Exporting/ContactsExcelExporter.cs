using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BTIT.EPM.DataExporting.Excel.EpPlus;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Dto;
using BTIT.EPM.Storage;

namespace BTIT.EPM.DigitalSignature.Exporting
{
    public class ContactsExcelExporter : EpPlusExcelExporterBase, IContactsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ContactsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetContactForViewDto> contacts)
        {
            return CreateExcelPackage(
                "Contacts.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Contacts"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("FirstName"),
                        L("LastName"),
                        L("Email"),
                        L("PhoneNumber")
                        );

                    AddObjects(
                        sheet, 2, contacts,
                        _ => _.Contact.FirstName,
                        _ => _.Contact.LastName,
                        _ => _.Contact.Email,
                        _ => _.Contact.PhoneNumber
                        );

					

                });
        }
    }
}
