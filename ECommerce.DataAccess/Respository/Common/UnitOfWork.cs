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

        public UnitOfWork(ECommerceDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            Product = new ProductRepository(_context, webHostEnvironment);
        }

        public IProductRepository Product { get; private set; }

        async Task IUnitOfWork.Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}