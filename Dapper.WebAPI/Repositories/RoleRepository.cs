using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Interfaces;
using Npgsql;

namespace Dapper.WebAPI.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IConfiguration configuration;
        public RoleRepository(IConfiguration configuration) 
        { 
            this.configuration = configuration;
        }
        public async Task<int> AddAsync(Roles entity)
        {
            var sql = "Insert into Roles (role_id,name,normalizedname,concurrencystamp) VALUES (@Role_id,@Name,@NormalizedName,@ConcurrencyStamp)";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }
        public async Task<int> DeleteAsync(string id)
        {
            var sql = "DELETE FROM Roles WHERE role_id = @Id";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Roles>> GetAllAsync()
        {
            var sql = "SELECT * FROM Roles";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Roles>(sql);
                return result.ToList();
            }
        }

        public async Task<Roles> GetByIdAsync(string id)
        {
            var sql = "SELECT * FROM Roles WHERE role_id = @Id";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<Roles>(sql, new { Id = id });
                return result;
            }
        }

        public Task<Roles> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(Roles entity)
        {
            var sql = "UPDATE Roles SET name = @Name,normalizedname = @NormalizedName,concurrencystamp = @ConcurrencyStamp WHERE role_id = @Role_id";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }
    }
}
