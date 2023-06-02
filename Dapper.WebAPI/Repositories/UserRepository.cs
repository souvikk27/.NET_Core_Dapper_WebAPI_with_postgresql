using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Entities.Relation;
using Dapper.WebAPI.Interfaces;
using Npgsql;

namespace Dapper.WebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration configuration;
        public UserRepository(IConfiguration configuration) { 
            this.configuration = configuration;
        }
        public async Task<int> AddAsync(Users entity)
        {
            entity.CreatedOn = DateTime.Now;
            var sql = "Insert into Users (Id,FirstName,LastName,UserName,Email,Passwordhash,CreatedOn,SecurityStamp) VALUES (@Id,@FirstName,@LastName,@UserName,@Email,@Password,@CreatedOn,@SecurityStamp)";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }

        public async Task<bool> CheckUser(string email, string password)
        {
            var sql = "SELECT * FROM Users WHERE email like @Email AND Passwordhash like @Password";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var response = await connection.QueryFirstOrDefaultAsync<Users>(sql ,new { Email = email,Password = password });
                return response != null;
            }
        }        

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Users WHERE Id = @Id";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }

        public async Task<IReadOnlyList<Users>> GetAllAsync()
        {
            var sql = "SELECT * FROM Users";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Users>(sql);
                return result.ToList();
            }
        }

        public async Task<Users> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Users WHERE Id = @Id";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<Users>(sql, new { Id = id });
                return result;
            }
        }

        public async Task<string> PasswordHash(string email)
        {
            var sql = "SELECT Passwordhash from Users WHERE Email = @Email";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<string>(sql, new { Email = email });
                string passwordHash = result;
                return passwordHash;
            }   
        }

        public async Task<int> UpdateAsync(Users entity)
        {
            var sql = "UPDATE Users SET FirstName = @FirstName,LastName = @LastName,UserName = @UserName,Email = @Email,Passwordhash = @Password WHERE Id = @Id";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }

        public async Task<string> SetUserRole(UserRoles entity)
        {
            const string sql = "Insert into UserRoles (userid,roleid) VALUES (@UserId, @RoleId)";
            using(var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql,entity);
                if(result == 1)
                return "User roles created successfully";
            }
            throw new NotImplementedException();
        }

        public async Task<List<UserRolesRelation>> GetUserRole()
        {
            List<UserRolesRelation> userRoles = new List<UserRolesRelation>();
            const string sql = "SELECT u.username, r.name FROM users u " +
                               "JOIN userroles ur ON u.id = ur.userid " +
                               "JOIN roles r ON ur.roleid = r.role_id ";
            using(var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var username = reader.GetString(0);
                            var rolename = reader.GetString(1);

                            var userRole = userRoles.FirstOrDefault(ur => ur.UserName == username);
                            if (userRole == null)
                            {
                                userRole = new UserRolesRelation
                                {
                                    UserName = username,
                                    RoleNames = new List<string>()
                                };
                                userRoles.Add(userRole);
                            }

                            userRole.RoleNames?.Add(rolename);
                        }
                    }
                }
                return  userRoles.ToList();
            }
            throw new NotImplementedException();
        }

        public async Task<UserRolesRelation> GetUserRoleById(string id)
        {
            UserRolesRelation userRoles = new UserRolesRelation();
            string sql = "SELECT u.username, r.name FROM users u " +
                               "JOIN userroles ur ON u.id = ur.userid " +
                               "JOIN roles r ON ur.roleid = r.role_id WHERE id = @Id";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var username = reader.GetString(0);
                            var rolename = reader.GetString(1);

                            if (userRoles.UserName == null)
                            {
                                userRoles.UserName = username;
                                userRoles.RoleNames = new List<string>();
                            }

                            userRoles.RoleNames?.Add(rolename);
                        }
                    }
                }
                return userRoles;
            }
            throw new NotImplementedException();
        }
    }
}
