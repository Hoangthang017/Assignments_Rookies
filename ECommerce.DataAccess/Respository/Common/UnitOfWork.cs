using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Respository.ProductRepo;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Respository.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ECommerceDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public UnitOfWork(ECommerceDbContext context, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            Product = new ProductRepository(_context, webHostEnvironment, mapper);
            ProductImage = new ProductImageRepository(_context, webHostEnvironment, mapper);
            ProductTranslation = new ProductTranslationRepository(_context, webHostEnvironment, mapper);
        }

        public IProductRepository Product { get; private set; }

        public IProductImageRepository ProductImage { get; private set; }

        public IProductTranslationRepository ProductTranslation { get; private set; }

        async Task IUnitOfWork.Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}