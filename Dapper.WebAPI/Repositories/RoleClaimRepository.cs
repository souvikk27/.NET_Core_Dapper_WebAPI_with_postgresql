using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Entities.Relation;
using Dapper.WebAPI.Interfaces;
using Npgsql;
using static Dapper.SqlMapper;

namespace Dapper.WebAPI.Repositories
{
    public class RoleClaimRepository : IRoleClaimRepository
    {
        private readonly IConfiguration configuration;
        public RoleClaimRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<int> AddRoleClaimsAsync(RoleClaims roleClaims)
        {
            const string sql = "INSERT INTO RoleClaims (RoleId,ClaimType,ClaimValue) VALUES (@RoleId,@ClaimType,@ClaimValue)";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, roleClaims);
                return result;
            }
            throw new NotImplementedException();
        }

        public async Task<int> RemoveRoleClaimsAsync(string roleId)
        {
            const string sql = "DELETE FROM RoleClaims WHERE roleid = @roleId";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var response = await connection.ExecuteAsync(sql, new { roleId = roleId });
                return response;
            }
            throw new NotImplementedException();
        }

        public async Task<RoleClaimRelation> GetRoleClaimsByIdAsync(string roleId)
        {
            const string sql = "SELECT rc.roleid,r.name,rc.claimtype,rc.claimvalue FROM roles r " +
                "JOIN roleclaims rc on r.role_id = rc.roleid WHERE r.role_id = @Id";
            RoleClaimRelation roleClaims = new RoleClaimRelation();
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("Id", roleId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while(reader.Read())
                        {
                            var rol_eId = reader.GetString(0);
                            var roleName = reader.GetString(1);
                            var claimType = reader.GetString(2);
                            var claimValue = reader.GetString(3);
                            if(roleClaims.RoleName == null)
                            {
                                roleClaims.RoleId = rol_eId;
                                roleClaims.RoleName = roleName;
                                roleClaims.ClaimList = new List<ClaimList>();
                            }
                            if (roleClaims.ClaimList == null)
                            {
                                roleClaims.ClaimList = new List<ClaimList>();
                            }
                            roleClaims.ClaimList.Add(new ClaimList
                            {
                                ClaimType = claimType,
                                ClaimValue = claimValue,
                                Selected = true
                            });
                        }
                    }
                }
                return roleClaims;
            }
                throw new NotImplementedException();
        }
    }
}
