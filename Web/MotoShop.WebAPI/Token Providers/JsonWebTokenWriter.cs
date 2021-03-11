using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MotoShop.WebAPI.Token_Providers
{
    public class JsonWebTokenWriter
    {
        private readonly IConfiguration _configuration;

        public JsonWebTokenWriter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string name, string value)
        {
            Claim[] claims = { new Claim(name, value) };
            return GenerateToken(claims);
        }
        public string GenerateToken(Claim[] claims)
        {

            var tokenDescriptor = new SecurityTokenDescriptor 
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(double.Parse(_configuration["JWT:Lifetime"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }
        public Claim[] AddStandardClaims(string userID, string role)
        {
            return new Claim[]
            {
                new Claim("UserID", userID),
                new Claim(ClaimTypes.Role, role)
            };

        }
        public Claim[] DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            return tokenS.Claims.ToArray();
        }
    }
}
