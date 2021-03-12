namespace MotoShop.WebAPI.Models.Response
{
    public class AuthenticatedResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
