using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.CommonRepo
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        private readonly ECommerceDbContext _context;

        public TagRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}