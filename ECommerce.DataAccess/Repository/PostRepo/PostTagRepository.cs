using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.OrderRepo
{
    public class PostTagRepository : Repository<PostTag>, IPostTagRepository
    {
        private readonly ECommerceDbContext _context;

        public PostTagRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}