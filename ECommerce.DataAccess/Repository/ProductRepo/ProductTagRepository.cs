using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class ProductTagRepository : Repository<ProductTag>, IProductTagRepository
    {
        private readonly ECommerceDbContext _context;

        public ProductTagRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}