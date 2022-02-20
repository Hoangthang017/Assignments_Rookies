using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.MenuRepo
{
    public class MenuGroupRepository : Repository<MenuGroup>, IMenuGroupRepository
    {
        private readonly ECommerceDbContext _context;

        public MenuGroupRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}