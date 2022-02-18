using AutoMapper;
using ECommerce.DataAccess.Entities;
using ECommerce.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.AutoMapper
{
    public class ECommerceMapperProfile : Profile
    {
        public ECommerceMapperProfile() : base()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductTranslation, ProductViewModel>();
        }
    }
}