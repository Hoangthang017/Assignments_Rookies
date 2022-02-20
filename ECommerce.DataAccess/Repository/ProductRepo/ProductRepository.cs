using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.ViewModels;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ECommerceDbContext _context;

        public ProductRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}