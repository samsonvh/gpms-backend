using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GPMS.Backend.Data.Models.Staffs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Utils
{
    public static class JWTUtils
    {

        private static IConfiguration _configuration;
        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static string GenerateJWTToken(Account account)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, account.Code.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Name, account.Staff.FullName),
                new Claim(ClaimTypes.Role, account.Staff.Position.ToString())
            };
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["JWT:Expires"])),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret_Key"])),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"]
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}