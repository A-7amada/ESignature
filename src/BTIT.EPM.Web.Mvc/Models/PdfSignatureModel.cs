using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Security.Authentication;

namespace BTIT.EPM.Web.Models
{
    public class PdfSignatureModel
    {
        public string Reason { get; set; }
        public string Location { get; set; }
        public string ContactInfo { get; set; }
        public HashAlgorithmType HashAlgorithm { get; set; } = HashAlgorithmType.Sha256;
        public Uri TSAServer { get; set; } = new Uri("https://freetsa.org/tsr");
    }
}

