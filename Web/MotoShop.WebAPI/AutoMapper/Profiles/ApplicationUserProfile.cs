using AutoMapper;
using MotoShop.Data.Models.User;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Models.Request;
using MotoShop.WebAPI.Models.Requests;
using MotoShop.WebAPI.Models.Response.UserAccount;

namespace MotoShop.WebAPI.AutoMapper.Profiles
{
    public class ApplicationUserProfile : Profile
    {
 
        public ApplicationUserProfile()
        {

            this.CreateMap<ApplicationUser, UserAccountDetailsResponseModel>().ForMember(x => x.Roles, x => x.Ignore());
            this.CreateMap<UpdateUserDataRequestModel, ApplicationUser>();
            this.CreateMap<UserRegisterRequestModel, ApplicationUser>();
        }

    }
}
