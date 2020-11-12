namespace MotoShop.WebAPI.Models.Requests
{
    public class NewPasswordRequestModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
