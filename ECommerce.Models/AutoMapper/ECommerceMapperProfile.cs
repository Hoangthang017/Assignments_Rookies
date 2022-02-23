using AutoMapper;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.ProductImages;
using ECommerce.Models.Request.Products;
using ECommerce.Models.ViewModels.ProductImages;
using ECommerce.Models.ViewModels.Products;

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
            CreateMap<ProductInCategory, ProductViewModel>();
            CreateMap<CategoryTranslation, ProductViewModel>()
                .ForMember(x => x.Categories, opt => opt.MapFrom(x => x.Name));
            CreateMap<ProductTranslation, ProductViewModel>();
            CreateMap<CategoryTranslation, ProductViewModel>();
            CreateMap<ProductImage, ProductImageViewModel>();
            CreateMap<UpdateProductImageRequest, ProductImage>();
        }
    }
}