namespace AuthService.Profiles
{
    using AuthService.Dtos;
    using AuthService.Models;
    using Mapster;
    using Microsoft.AspNetCore.Identity;

    public class MapsterProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // in my case i will use email as username
            // also changing email will change the username
            config.NewConfig<Register, IdentityUser>()
                .Map(dest => dest.UserName, src => src.Email)
                .Map(dest => dest.Email, src => src.Email);
        }
    }
}
