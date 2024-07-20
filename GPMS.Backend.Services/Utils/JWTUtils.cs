using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GPMS.Backend.Data.Enums.Others;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
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
                new Claim("StaffId",account.Staff.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, account.Code.ToString()),
                new Claim(ClaimTypes.Name, account.Staff.FullName),
                new Claim(ClaimTypes.Role, account.Staff.Position.ToString()),
            };
            if(!account.Staff.Position.Equals(StaffPosition.Admin))
            {
                claims.Add(new Claim(ClaimTypes.Role, account.Staff.Position.ToString()));
            }
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

        public static CurrentLoginUserDTO DecryptAccessToken(string accessToken)
        {
            string accessTokenPrefix = "Bearer ";
            if (accessToken.Contains(accessTokenPrefix))
            {
                accessToken = accessToken.Substring(accessTokenPrefix.Length);
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret_Key"])),
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidAudience = _configuration["JWT:Audience"]
            };
            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(accessToken.Trim(), tokenValidationParameters, out _);
            if (claimsPrincipal == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Failed to decrypt/validate access token");
            }
            return new CurrentLoginUserDTO 
            {
                StaffId = Guid.Parse(claimsPrincipal.FindFirstValue("StaffId")),
                Code = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier),
                Department = claimsPrincipal.FindFirstValue("Department"),
                FullName = claimsPrincipal.FindFirstValue(ClaimTypes.Name),
                Position = claimsPrincipal.FindFirstValue(ClaimTypes.Role)
            };
        }
    }
}