using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.CommonRepo
{
    public class SystemConfigRepository : Repository<SystemConfig>, ISystemConfigRepository
    {
        private readonly ECommerceDbContext _context;

        public SystemConfigRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}