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
            CreateMap<CreateProductRequest, Product>();
            CreateMap<CreateProductRequest, ProductTranslation>();
            CreateMap<UpdateProductRequest, ProductTranslation>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductInCategory, ProductViewModel>()
                .ForMember(x => x.CatogetyId, opt => opt.MapFrom(x => x.CategoryId));
            CreateMap<ProductTranslation, ProductViewModel>();
            CreateMap<CategoryTranslation, ProductViewModel>();
        }
    }
}