using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ECommerceDbContext _context;

        public CategoryRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}