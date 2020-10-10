using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MotoShop.Data.Models.User;
using MotoShop.WebAPI.Models.Response.UserAccount;

namespace MotoShop.WebAPI.AutoMapper.Profiles
{
    public class ApplicationUserProfile : Profile
    {

        public ApplicationUserProfile()
        {
            this.CreateMap<ApplicationUser, UserAccountDetailsResponseModel>();

        }
    }
}
