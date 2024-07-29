using AuthService.Dtos;
using AuthService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<Register, IdentityUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));


            CreateMap<Register, RegisterUserDto>();
        }
    }
}
