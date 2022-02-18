using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Entities;
using ECommerce.DataAccess.Respository.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Respository.ProductRepo
{
    public class ProductTranslationRepository : Repository<ProductTranslation>, IProductTranslationRepository
    {
        private readonly ECommerceDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public ProductTranslationRepository(ECommerceDbContext context, IWebHostEnvironment webHostEnvironment, IMapper mapper) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public async Task<ProductTranslation> GetByProductId(int id)
        {
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == id);
            return productTranslation;
        }
    }
}