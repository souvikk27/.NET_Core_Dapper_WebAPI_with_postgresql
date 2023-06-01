using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Entities.Relation;

namespace Dapper.WebAPI.Interfaces
{
    public interface IUserRepository : IGenericRepository<Users>
    {
        public Task<bool> CheckUser(string Email, string Password);
        public Task<string> PasswordHash(string Email);
        public Task<string> SetUserRole(UserRoles entity);

        public Task<List<UserRolesRelation>> GetUserRole();
    }
}
