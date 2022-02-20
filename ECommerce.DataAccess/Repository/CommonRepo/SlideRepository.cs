using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.CommonRepo
{
    public class SlideRepository : Repository<Slide>, ISlideRepository
    {
        private readonly ECommerceDbContext _context;

        public SlideRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}