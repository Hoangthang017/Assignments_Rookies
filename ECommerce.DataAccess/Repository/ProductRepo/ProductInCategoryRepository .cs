using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class ProductInCategoryRepository : Repository<ProductInCategory>, IProductInCategoryRepository
    {
        private readonly ECommerceDbContext _context;

        public ProductInCategoryRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}