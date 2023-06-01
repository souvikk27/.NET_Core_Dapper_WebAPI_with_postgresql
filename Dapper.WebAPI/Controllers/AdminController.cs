using Dapper.WebAPI.Data_Transfer_Objects;
using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Helpers;
using Dapper.WebAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;
using System.Reflection;

namespace Dapper.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordEncrypt _passwordEncrypt;

        public AdminController(IUnitOfWork unitOfWork , PasswordEncrypt passwordEncrypt)
        {
            _unitOfWork = unitOfWork;
            _passwordEncrypt = passwordEncrypt;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto == null)
            {
                return Ok();
            }
            var passwordHash = _passwordEncrypt.GeneratePasswordHash(dto.Password);
            Users user = new()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Email = dto.Email,
                Password = passwordHash,
                CreatedOn = DateTime.UtcNow,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var response = await _unitOfWork.Users.AddAsync(user);
            return Ok("User Registration is Successful");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            Users user  = new()
            {
                Email = dto.Email,
                Password = dto.Password
            };
            var passwordHash = await _unitOfWork.Users.PasswordHash(user.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(user.Password, passwordHash))
            {
                return BadRequest("Credentials does not match");
            }

            return Ok("Logged in successfully");
        }

        [HttpPost]
        [Route("setUserRole")]
        public async Task<IActionResult> AssignUserRole(UserRolesDto dto)
        {
            foreach (var selectedRole in dto.roles)
            {
                UserRoles userRole = new()
                {
                    userid = dto.userid,
                    roleid = selectedRole.roleid
                };
                var response = await _unitOfWork.Users.SetUserRole(userRole);
                if(response == null)
                {
                    return BadRequest();
                }
            }
            return Ok();
        }

        [HttpGet]
        [Route("getUserRole")]
        public async Task<IActionResult> GetAllUserRole()
        {
            var data = await _unitOfWork.Users.GetUserRole();
            return Ok(data);
        }
    }
}
