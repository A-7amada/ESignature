using System;
using System.Collections.Generic;
using System.Text;

namespace BTIT.EPM.Documents.Dtos
{
    public class FileUploadData
    {
        public long DocumentBagId { get; set; }

        public List<DocumentDto> Documents { get; set; }

    }
}
