using AutoMapper;
using MotoShop.Data.Models.Store;
using MotoShop.WebAPI.Models.Response.ItemsController;

namespace MotoShop.WebAPI.AutoMapper.Profiles
{
    public class ItemsProfile : Profile
    {
        public ItemsProfile()
        {
            this.CreateMap<Car, CarDetailsResponseModel>();
        }
    }
}
