using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.OrderRepo
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ECommerceDbContext _context;

        public OrderDetailRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}