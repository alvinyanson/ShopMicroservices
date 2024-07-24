using AutoMapper;
using ProductCatalogService.Dtos;
using ProductCatalogService.Models;

namespace ProductCatalogService.Profiles
{
    public class ProductCatalogProfile : Profile
    {
        public ProductCatalogProfile()
        {
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
        }
    }
}
