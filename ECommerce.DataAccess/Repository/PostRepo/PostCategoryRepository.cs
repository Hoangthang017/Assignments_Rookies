using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.OrderRepo
{
    public class PostCategoryRepository : Repository<PostCategory>, IPostCategoryRepository
    {
        private readonly ECommerceDbContext _context;

        public PostCategoryRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}