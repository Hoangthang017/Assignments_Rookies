using AutoMapper;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Categories;
using ECommerce.Models.Request.Products;
using ECommerce.Models.ViewModels.Products;
using ECommerce.Models.ViewModels.UserInfos;

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
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Name));
            CreateMap<ProductTranslation, ProductViewModel>();
            CreateMap<CategoryTranslation, ProductViewModel>();

            CreateMap<User, UserInfoViewModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => (x.LastName + " " + x.FirstName)))
                .ForAllMembers(x => x.UseDestinationValue());

            CreateMap<Role, UserInfoViewModel>()
                .ForMember(x => x.Role, opt => opt.MapFrom(x => x.Name));

            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<CreateCategoryRequest, CategoryTranslation>();
            CreateMap<UpdateCategoryRequest, Category>();
        }
    }
}