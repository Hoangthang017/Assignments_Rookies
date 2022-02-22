using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class ProductTranslationRepository : Repository<ProductTranslation>, IProductTranslationRepository
    {
        private readonly ECommerceDbContext _context;

        public ProductTranslationRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}