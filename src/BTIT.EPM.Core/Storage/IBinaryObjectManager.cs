using System;
using System.Threading.Tasks;

namespace BTIT.EPM.Storage
{
    public interface IBinaryObjectManager
    {
        Task<BinaryObject> GetOrNullAsync(Guid id);
        
        Task SaveAsync(BinaryObject file);

        Task<Guid> SaveAndGetIdAsync(BinaryObject file);


        Task DeleteAsync(Guid id);
    }
}