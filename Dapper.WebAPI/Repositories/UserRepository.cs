using Dapper.WebAPI.Entities;
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
            var sql = "Insert into Users (FirstName,LastName,UserName,Email,Passwordhash,CreatedOn,SecurityStamp) VALUES (@FirstName,@LastName,@UserName,@Email,@Password,@CreatedOn,@SecurityStamp)";
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

        
    }
}
