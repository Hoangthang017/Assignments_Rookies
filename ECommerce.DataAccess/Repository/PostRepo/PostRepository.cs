using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.OrderRepo
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ECommerceDbContext _context;

        public PostRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}