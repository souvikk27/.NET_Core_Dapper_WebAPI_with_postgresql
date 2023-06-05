using Dapper.WebAPI.Data_Transfer_Objects;
using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace Dapper.WebAPI.Controllers
{
    [Route("api/v1/roles")]
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
        
        [HttpPut]
        [Route("updateRoleClaims/{id}")]
        public async Task<IActionResult> UpdateRoleClaimAsync([FromBody] RoleClaimsDto dto)
        {
            var claims = await unitOfWork.RoleClaims.GetRoleClaimsByIdAsync(dto.RoleId);
            if(claims != null)
                await unitOfWork.RoleClaims.RemoveRoleClaimsAsync(dto.RoleId);

            var selectedClaims = dto.claims.Where(a => a.Selected == true);
            int counter = 0;
            foreach(var claim in selectedClaims)
            {
                RoleClaims roleClaims = new RoleClaims
                {
                    RoleId = dto.RoleId,
                    ClaimType = claim.ClaimType,
                    ClaimValue = claim.ClaimValue,
                };
                var response = await unitOfWork.RoleClaims.AddRoleClaimsAsync(roleClaims);
                if (response > 0)
                    counter++;
            }
            if (counter > 0)
                return Ok(counter + " claims added for current role");
            return BadRequest("Invalid Operation") ;
        }

        //[HttpGet]
        //[Route("getRoleClaims")]
        //public async Task<IActionResult> GetAllRoleClaims()
        //{
        //    var data = await unitOfWork.RoleClaims.GetAllRoleClaimsAsync();
        //    return Ok(data);
        //}
        [HttpGet]
        [Route("getRoleClaims/{id}")]
        public async Task<IActionResult> GetRoleClaimsById(string id)
        {
            var data = await unitOfWork.RoleClaims.GetRoleClaimsByIdAsync(id);
            return Ok(data);
        }
    }
}
