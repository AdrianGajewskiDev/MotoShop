using System.Security.Claims;

namespace MotoShop.WebAPI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserID(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(x => x.Type == "UserID").Value;
        }
    }
}
