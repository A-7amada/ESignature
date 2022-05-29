using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BTIT.EPM.DataExporting.Excel.NPOI;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Dto;
using BTIT.EPM.Storage;

namespace BTIT.EPM.DigitalSignature.Exporting
{
    public class RecipientsExcelExporter : NpoiExcelExporterBase, IRecipientsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RecipientsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRecipientForViewDto> recipients)
        {
            return CreateExcelPackage(
                "Recipients.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Recipients"));

                    AddHeader(
                        sheet,
                        L("Type"),
                        L("FirstName"),
                        L("LastName"),
                        L("Email"),
                        L("IsSigner"),
                        L("Code"),
                        L("ViewDate"),
                        L("SignatureDate"),
                        L("SignerPin"),
                        L("IsSigned"),
                        L("SigneOrder"),
                        L("FieldName"),
                        L("SentDate"),
                        L("IsSent"),
                        L("SignerPinTriesCount"),
                        (L("User")) + L("Name"),
                        (L("DocumentRequest")) + L("DocumentTitle")
                        );

                    AddObjects(
                        sheet, 2, recipients,
                        _ => _.Recipient.Type,
                        _ => _.Recipient.FirstName,
                        _ => _.Recipient.LastName,
                        _ => _.Recipient.Email,
                        _ => _.Recipient.IsSigner,
                        _ => _.Recipient.Code,
                        _ => _timeZoneConverter.Convert(_.Recipient.ViewDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Recipient.SignatureDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Recipient.SignerPin,
                        _ => _.Recipient.IsSigned,
                        _ => _.Recipient.SigneOrder,
                        _ => _.Recipient.FieldName,
                        _ => _timeZoneConverter.Convert(_.Recipient.SentDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Recipient.IsSent,
                        _ => _.Recipient.SignerPinTriesCount,
                        _ => _.UserName,
                        _ => _.DocumentRequestDocumentTitle
                        );

					
					for (var i = 1; i <= recipients.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[7], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(7);for (var i = 1; i <= recipients.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[8], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(8);for (var i = 1; i <= recipients.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[13], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(13);
                });
        }
    }
}
