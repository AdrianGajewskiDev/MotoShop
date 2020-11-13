namespace MotoShop.WebAPI.Models.Requests
{
    public class ResetPasswordRequestModel
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
