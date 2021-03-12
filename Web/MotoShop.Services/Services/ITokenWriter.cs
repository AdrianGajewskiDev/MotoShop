using System.Security.Claims;

namespace MotoShop.Services.Services
{
    public interface ITokenWriter
    {
        public string GenerateToken(string name, string value);
        public string GenerateToken(Claim[] claims);
        public Claim[] AddStandardClaims(string userID, string role);
        public Claim[] DecodeToken(string token);
    }
}
