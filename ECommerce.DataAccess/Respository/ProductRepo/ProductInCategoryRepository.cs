using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Entities;
using ECommerce.DataAccess.Respository.Common;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Respository.ProductRepo
{
    public class ProductInCategoryRepository : Repository<ProductInCategory>, IProductInCategoryRepository
    {
        private readonly ECommerceDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public ProductInCategoryRepository(ECommerceDbContext context, IWebHostEnvironment webHostEnvironment, IMapper mapper) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }
    }
}