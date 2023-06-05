using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Entities.Relation;

namespace Dapper.WebAPI.Interfaces
{
    public interface IRoleClaimRepository
    {
        public Task<int> AddRoleClaimsAsync(RoleClaims roleClaims);

        public Task<List<RoleClaimRelation>> GetAllRoleClaimsAsync();

        public Task<RoleClaimRelation> GetRoleClaimsByIdAsync(string roleId);
    }
}
