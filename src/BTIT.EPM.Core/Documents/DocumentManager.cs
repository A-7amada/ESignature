using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTIT.EPM.Documents
{
    public class DocumentManager : IDocumentManager
    {
        private readonly IRepository<Document, long> _documentRepository;
        private readonly IRepository<DocumentBag, long> _documentBagRepository;

        public DocumentManager(IRepository<Document, long> documentRepository, IRepository<DocumentBag, long> documentBagRepository)
        {
            _documentRepository = documentRepository;
            _documentBagRepository = documentBagRepository;
        }
        public long GenerateDocmantBagId()
        {
            var documentBagId = _documentBagRepository.InsertAndGetId(new DocumentBag());
            return documentBagId;
        }

        public async Task<List<Document>> GetDocuments(long documentBagId)
        {
            var documents = await  _documentRepository.GetAll().Where(e=>e.DocumentBagId == documentBagId).ToListAsync();
            return documents;
        }
    }
}
