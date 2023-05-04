using Dapper.WebAPI.Entities;

namespace Dapper.WebAPI.Interfaces
{
    public interface IProductRepository : IGenericRepository<Products>
    {
        Task<int> BulkUpdateFromFileAsync(string filePath);
    }
}
