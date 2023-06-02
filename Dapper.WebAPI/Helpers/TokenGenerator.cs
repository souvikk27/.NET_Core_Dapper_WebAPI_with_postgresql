using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Interfaces;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Dapper.WebAPI.Helpers
{

    public class TokenGenerator
    {
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitOfWork;
        public TokenGenerator(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            this.configuration = configuration;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Token> GenerateJwtToken(string email)
        {
            var user = await unitOfWork.Users.FindByEmail(email);
            if (user != null)
            {
                var userRoles = await unitOfWork.Users.GetUserRoleById(user.Id);
                var authClaims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                    new Claim(ClaimTypes.Name,user.FirstName,user.LastName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };
                foreach (var userRole in userRoles.RoleNames)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var authSecurity = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSecurity, SecurityAlgorithms.HmacSha256)
                    );
                //return new JwtSecurityTokenHandler().WriteToken(token);
                return new Token
                {
                    accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
            };
            }
            throw new InvalidOperationException("User not found.");

        }
    }
}
