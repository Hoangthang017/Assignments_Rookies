using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class ProductReviewRepository : Repository<ProductReview>, IProductReviewRepository
    {
        private readonly ECommerceDbContext _context;

        public ProductReviewRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}