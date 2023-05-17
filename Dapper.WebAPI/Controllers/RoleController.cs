using Dapper.WebAPI.Data_Transfer_Objects;
using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dapper.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public RoleController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var data = await unitOfWork.Roles.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var data = await unitOfWork.Roles.GetByIdAsync(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RolesDto dto)
        {
            Roles roles = new Roles
            {
                Role_Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                NormalizedName = dto.Name.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            var response = await unitOfWork.Roles.AddAsync(roles);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var data = await unitOfWork.Roles.DeleteAsync(id);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RolesDto dto)
        {
            Roles roles = new Roles
            {
                Role_Id = dto.Role_Id,
                Name = dto.Name,
                NormalizedName = dto.Name.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            var response = await unitOfWork.Roles.UpdateAsync(roles);
            return Ok(response);
        }
    }
}
