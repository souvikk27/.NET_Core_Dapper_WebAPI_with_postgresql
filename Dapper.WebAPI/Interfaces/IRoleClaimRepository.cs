using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Entities.Relation;

namespace Dapper.WebAPI.Interfaces
{
    public interface IRoleClaimRepository
    {
        public Task<int> AddRoleClaimsAsync(RoleClaims roleClaims);

        public Task<RoleClaimRelation> GetRoleClaimsByIdAsync(string roleId);

        public Task<int> RemoveRoleClaimsAsync(string roleId);
    }
}
