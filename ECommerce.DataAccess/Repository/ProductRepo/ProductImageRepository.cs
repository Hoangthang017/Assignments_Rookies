using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private readonly ECommerceDbContext _context;

        public ProductImageRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}