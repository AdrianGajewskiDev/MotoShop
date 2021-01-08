using AutoMapper;
using MotoShop.Data.Models.User;
using MotoShop.WebAPI.Models.Request;
using MotoShop.WebAPI.Models.Requests;
using MotoShop.WebAPI.Models.Response.UserAccount;

namespace MotoShop.WebAPI.AutoMapper.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            this.CreateMap<ApplicationUser, UserAccountDetailsResponseModel>();
            this.CreateMap<UpdateUserDataRequestModel, ApplicationUser>();
            this.CreateMap<UserRegisterRequestModel, ApplicationUser>();
        }
    }
}
