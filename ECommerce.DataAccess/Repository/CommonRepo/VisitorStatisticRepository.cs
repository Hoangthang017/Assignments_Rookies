using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.CommonRepo
{
    public class VisitorStatisticRepository : Repository<VisitorStatistic>, IVisitorStatisticRepository
    {
        private readonly ECommerceDbContext _context;

        public VisitorStatisticRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}