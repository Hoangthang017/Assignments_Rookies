using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class CategoryTranslationRepository : Repository<CategoryTranslation>, ICategoryTranslationRepository
    {
        private readonly ECommerceDbContext _context;

        public CategoryTranslationRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}