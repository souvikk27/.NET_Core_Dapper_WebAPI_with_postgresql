using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Interfaces;
using Npgsql;
using OfficeOpenXml;
using System.Data.SqlClient;

namespace Dapper.WebAPI.Repositories
{
    
    
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration configuration;
        public ProductRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<int> AddAsync(Products entity)
        {
            entity.AddedOn = DateTime.Now;
            var sql = "Insert into Products (Name,Description,Barcode,Rate,AddedOn) VALUES (@Name,@Description,@Barcode,@Rate,@AddedOn)";
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Products WHERE Id = @Id" ;
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }

        public async Task<IReadOnlyList<Products>> GetAllAsync()
        {
            var sql = "SELECT * FROM Products";
            using(var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Products>(sql);
                return result.ToList();
            }
        }

        public async Task<Products> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Products WHERE Id = @Id";
            using(var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<Products>(sql, new {Id = id });
                return result;
            }
        }

        public async Task<int> UpdateAsync(Products entity)
        {
            entity.ModifiedOn = DateTime.Now;
            var sql = "UPDATE Products SET Name = @Name, Description = @Description, Barcode = @Barcode, Rate = @Rate, ModifiedOn = @ModifiedOn  WHERE Id = @Id";
            using(var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql,entity);
                return result;
            }
        }
        public async Task<int> ImportExcelFileAsync(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets[0];

                var products = new List<Products>();

                // Skip header row
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    
                    var product = new Products
                    {
                        Name = worksheet.Cells[row, 2].Value.ToString(),
                        Barcode = worksheet.Cells[row, 3].Value.ToString(),
                        Description = worksheet.Cells[row, 4].Value.ToString(),
                        Rate = decimal.Parse(worksheet.Cells[row, 5].Value.ToString()),
                        AddedOn = DateTime.Now,
                    };

                    products.Add(product);
                }

                // Save products to database
                using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
                {
                    connection.Open();
                    var result = 0;
                    foreach (var product in products)
                    {
                        var existingProduct = await connection.QueryFirstOrDefaultAsync<Products>("SELECT * FROM Products WHERE Barcode = @Barcode", new { Barcode = product.Barcode });

                        if (existingProduct == null)
                        {
                            var sql = "INSERT INTO Products (Name, Description, Barcode, Rate, AddedOn) VALUES (@Name, @Description, @Barcode, @Rate, @AddedOn)";
                            result = await connection.ExecuteAsync(sql, product);
                        }
                    }
                    return result;
                }
            }
        }



    }
}
