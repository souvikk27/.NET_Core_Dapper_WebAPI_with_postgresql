using Dapper.WebAPI.Entities;

namespace Dapper.WebAPI.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Roles>
    {
        public Task<int> DeleteAsync(string id);
        public Task<Roles> GetByIdAsync(string id);


    }
}
