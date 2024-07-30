using AutoMapper;
using ShopWebApp.Dtos;
using ShopWebApp.Models;

namespace ShopWebApp.Profiles
{
    public class ShopWebAppProfile : Profile
    {
        public ShopWebAppProfile()
        {
            CreateMap<Register, Login>();
            CreateMap<Register, RegisterUserDto>();


            CreateMap<Cart, AddItemToCartDto>();
        }
    }
}
