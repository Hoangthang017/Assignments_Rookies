using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.CommonRepo
{
    public class SupportOnlineRepository : Repository<SupportOnline>, ISupportOnlineRepository
    {
        private readonly ECommerceDbContext _context;

        public SupportOnlineRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}