using Dapper.WebAPI.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using OfficeOpenXml;
using System.Xml.Serialization;

namespace Dapper.WebAPI.Helpers
{
    public class ExcelOperations
    {
        private readonly IConfiguration configuration;
        public ExcelOperations(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<int> ImportExcelFile(string filePath)
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
                        ModifiedOn = DateTime.Now,
                    };

                    products.Add(product);
                }

                // Save products to database
                using (var connection = new NpgsqlConnection(configuration.GetConnectionString("ConnStr")))
                {
                    connection.Open();
                    var result = 0;
                    // Upload or update Products in the database
                    foreach (var product in products)
                    {
                        var existingProduct = await connection.QueryFirstOrDefaultAsync<Products>("SELECT * FROM Products WHERE Barcode = @Barcode", new { Barcode = product.Barcode });
                        if (existingProduct == null)
                        {
                            var sql = "INSERT INTO Products (Name, Description, Barcode, Rate, AddedOn) VALUES (@Name, @Description, @Barcode, @Rate, @AddedOn)";
                            result = await connection.ExecuteAsync(sql, product);
                        }
                        if (existingProduct != null)
                        {
                            var sql = "UPDATE Products SET Name = @Name, Description = @Description, Rate = @Rate, ModifiedOn = @ModifiedOn Where Barcode = @Barcode";
                            result = await connection.ExecuteAsync(sql, product);
                        }
                    }
                    //Delete Products from Database if Product is removed from Excel file
                    var barcodesFromFile = products.Select(p => p.Barcode).ToList();
                    var barcodesFromDb = (await connection.QueryAsync<string>("SELECT Barcode FROM Products")).ToList();
                    var missingBarcodes = barcodesFromDb.Except(barcodesFromFile).ToList();
                    foreach (var barcode in missingBarcodes)
                    {
                        var sql = "DELETE FROM Products WHERE Barcode = @Barcode";
                        await connection.ExecuteAsync(sql, new { Barcode = barcode });
                    }
                    return result;

                }
            }
        }
    }
}
