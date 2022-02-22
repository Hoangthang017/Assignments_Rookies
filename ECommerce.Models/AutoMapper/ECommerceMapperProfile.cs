using AutoMapper;
using ECommerce.Models.Entities;
using ECommerce.Models.Request;
using ECommerce.Models.ViewModels;

namespace ECommerce.Models.AutoMapper
{
    public class ECommerceMapperProfile : Profile
    {
        public ECommerceMapperProfile()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>();

            CreateMap<Product, Product>();
        }
    }
}