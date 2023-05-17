using Dapper.WebAPI.Entities;

namespace Dapper.WebAPI.Interfaces
{
    public interface IUserRepository : IGenericRepository<Users>
    {
        public Task<bool> CheckUser(string Email, string Password);
        public Task<string> PasswordHash(string Email);
    }
}
