using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dapper.WebAPI.Controllers
{
    [Route("api/v1/items")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet,Authorize]
        public async Task<IActionResult> GetAll()
        {
            var data = await unitOfWork.Products.GetAllAsync();
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await unitOfWork.Products.GetByIdAsync(id);
            if (data == null) return Ok();
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> Add(Products product)
        {
            var data = await unitOfWork.Products.AddAsync(product);
            return Ok(data);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await unitOfWork.Products.DeleteAsync(id);
            return Ok(data);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Products product)
        {
            var data = await unitOfWork.Products.UpdateAsync(product);
            return Ok(data);
        }
        [HttpPost("BulkUpload")]
        public async Task<IActionResult> BulkUploadProduct(string filePath)
        {
            var data = await unitOfWork.Products.BulkUpdateFromFileAsync(filePath);
            return Ok(data);
        }
    }
}