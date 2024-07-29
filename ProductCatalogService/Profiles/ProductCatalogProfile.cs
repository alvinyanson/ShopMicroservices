using AutoMapper;
using ProductCatalogService.Dtos;
using ProductCatalogService.Models;

namespace ProductCatalogService.Profiles
{
    public class ProductCatalogProfile : Profile
    {
        public ProductCatalogProfile()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();


            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();


            CreateMap<AddItemToCartDto, Cart>();
        }
    }
}
