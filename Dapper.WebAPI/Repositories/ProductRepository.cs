﻿using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Helpers;
using Dapper.WebAPI.Interfaces;
using Npgsql;
using OfficeOpenXml;
using System.Data.SqlClient;

namespace Dapper.WebAPI.Repositories
{
    
    
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration configuration;
        private readonly ExcelOperations excelOperations;
        public ProductRepository(IConfiguration configuration, ExcelOperations excelOperations)
        {
            this.configuration = configuration;
            this.excelOperations = excelOperations;
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

        //This Function Takes Care Of Entire Create, Upload, Delete Opreation Based on Excel Modification
        public async Task<int> BulkUpdateFromFileAsync(string filePath)
        {
            return await excelOperations.ImportExcelFile(filePath);
        }
    }
}
