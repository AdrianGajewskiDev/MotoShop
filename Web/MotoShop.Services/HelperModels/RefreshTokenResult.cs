using System.Collections.Generic;

namespace MotoShop.Services.HelperModels
{
    public class RefreshTokenResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
