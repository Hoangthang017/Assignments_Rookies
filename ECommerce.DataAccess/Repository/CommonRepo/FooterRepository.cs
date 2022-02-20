using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.CommonRepo
{
    public class FooterRepository : Repository<Footer>, IFooterRepository
    {
        private readonly ECommerceDbContext _context;

        public FooterRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}