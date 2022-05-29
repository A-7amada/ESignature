using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;

namespace BTIT.EPM.Storage
{
    public class DbBinaryObjectManager : IBinaryObjectManager, ITransientDependency
    {
        private readonly IRepository<BinaryObject, Guid> _binaryObjectRepository;

        public DbBinaryObjectManager(IRepository<BinaryObject, Guid> binaryObjectRepository)
        {
            _binaryObjectRepository = binaryObjectRepository;
        }

        public Task<BinaryObject> GetOrNullAsync(Guid id)
        {
            return _binaryObjectRepository.FirstOrDefaultAsync(id);
        }

        public Task SaveAsync(BinaryObject file)
        {
            return _binaryObjectRepository.InsertAsync(file);
        }

        public Task<Guid> SaveAndGetIdAsync(BinaryObject file)
        {
             var id = _binaryObjectRepository.InsertAndGetIdAsync(file);
            return id;
        }

        public Task DeleteAsync(Guid id)
        {
            return _binaryObjectRepository.DeleteAsync(id);
        }
    }
}