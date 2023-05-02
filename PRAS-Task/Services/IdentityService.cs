using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PRAS_Task.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRAS_Task.Services
{
    public class IdentityService: IIdentityService
    {
        private readonly IConfiguration _config;

        public IdentityService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public LoginResponse? Login(IdentityUser user)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var token = GetToken(authClaims);

            return new LoginResponse()
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = user.Email
            };
        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var token = new JwtSecurityToken(
                expires: DateTime.Now.Add(TimeSpan.FromMinutes(Convert.ToInt32(_config["JWT:Lifetime"]))),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
