using BTIT.EPM.ESignatureDemo.Dtos;

using Abp.Extensions;
using BTIT.EPM.Documents.Dtos;
using System.Collections.Generic;

namespace BTIT.EPM.Web.Areas.App.Models.FileSignatures
{
    public class CreateOrEditFileSignatureModalViewModel
    {
        public CreateOrEditFileSignatureDto FileSignature { get; set; }
        public List<DocumentDto> Documents { get; set; }
        public bool IsEditMode => FileSignature.Id.HasValue;
    }
}