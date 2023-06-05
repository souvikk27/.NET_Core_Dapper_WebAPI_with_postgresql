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

        public async Task<List<RoleClaimRelation>> GetAllRoleClaimsAsync()
        {
            List<RoleClaimRelation> roleClaims = new List<RoleClaimRelation>();
            const string sql = "SELECT r.name,rc.claimtype,rc.claimvalue FROM roles r " +
                "JOIN roleclaims rc on r.role_id = rc.roleid";
            using(var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                using(var command = new NpgsqlCommand(sql, connection))
                {
                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        while(reader.Read())
                        {
                            var roleName = reader.GetString(0);
                            var claimType = reader.GetString(1);
                            var claimValue = reader.GetString(2);
                            var roleClaim = roleClaims.FirstOrDefault(rc => rc.RoleName == roleName);
                            if(roleClaim == null)
                            {
                                roleClaim = new RoleClaimRelation
                                {
                                    RoleName = roleName,
                                    ClaimType = claimType,
                                    ClaimValue = new List<string>()
                                };
                                roleClaims.Add(roleClaim);
                            }
                            roleClaim.ClaimValue?.Add(claimValue);
                        }
                    }
                }
                return roleClaims.ToList();
            }
            throw new NotImplementedException();
        }

        public async Task<RoleClaimRelation> GetRoleClaimsByIdAsync(string roleId)
        {
            const string sql = "SELECT r.name,rc.claimtype,rc.claimvalue FROM roles r " +
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
                            var roleName = reader.GetString(0);
                            var claimType = reader.GetString(1);
                            var claimValue = reader.GetString(2);
                            if(roleClaims.RoleName == null)
                            {
                                roleClaims.RoleName = roleName;
                                roleClaims.ClaimType = claimType;
                                roleClaims.ClaimValue = new List<string>();
                            }
                            roleClaims.ClaimValue?.Add(claimValue);
                        }
                    }
                }
                return roleClaims;
            }
                throw new NotImplementedException();
        }
    }
}
