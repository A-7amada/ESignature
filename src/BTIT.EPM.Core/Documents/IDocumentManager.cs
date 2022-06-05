using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTIT.EPM.Documents
{
    public interface IDocumentManager : IDomainService
    {
        long GenerateDocmantBagId();
        Task<List<Document>> GetDocuments(long documentBagId);
    }
}
