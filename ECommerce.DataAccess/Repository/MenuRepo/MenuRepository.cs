using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.MenuRepo
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private readonly ECommerceDbContext _context;

        public MenuRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}