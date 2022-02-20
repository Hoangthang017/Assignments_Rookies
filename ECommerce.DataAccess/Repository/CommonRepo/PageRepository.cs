using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.CommonRepo
{
    public class PageRepository : Repository<Page>, IPageRepository
    {
        private readonly ECommerceDbContext _context;

        public PageRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}