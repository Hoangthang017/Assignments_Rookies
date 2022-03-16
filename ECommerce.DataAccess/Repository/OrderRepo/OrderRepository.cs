using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Orders;
using ECommerce.Models.ViewModels.Orders;
using ECommerce.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DataAccess.Repository.OrderRepo
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ECommerceDbContext _context;

        public OrderRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> Create(int productId, Guid customerId, CreateOrderRequest request)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new ECommerceException("Cannot find the product");

            // check has customer and handle if it is anonymous account
            if (customerId == Guid.Empty) customerId = SystemConstants.AnonymousAccountSettings.Id;
            var customer = await _context.Users.FindAsync(customerId);
            if (customer == null) throw new ECommerceException("Cannot find the customer");

            var order = new Order()
            {
                ShipName = request.ShipName,
                ShipAddress = request.ShipAddress,
                ShipEmail = request.ShipEmail,
                ShipPhoneNumber = request.ShipPhoneNumber,
                OrderDate = DateTime.Now,
                Status = request.Status,
                User = customer,
            };

            var orderDetail = new OrderDetail()
            {
                Order = order,
                OrderId = order.Id,
                ProductId = productId,
                Product = product,
                Price = request.Price,
                Quantity = request.Quantity
            };

            order.OrderDetails = new List<OrderDetail>() { orderDetail };

            await _context.Orders.AddAsync(order);

            await _context.SaveChangesAsync();

            return order.Id;
        }

        public async Task<OrderViewModel> GetById(Guid customerId, int orderId)
        {
            if (customerId == Guid.Empty) customerId = SystemConstants.AnonymousAccountSettings.Id;
            var order = await (from o in _context.Orders
                               join od in _context.OrderDetails on o.Id equals od.OrderId
                               where o.Id == orderId && o.UserId == customerId
                               select new OrderViewModel()
                               {
                                   Id = o.Id,
                                   ShipName = o.ShipName,
                                   ShipAddress = o.ShipAddress,
                                   ShipEmail = o.ShipEmail,
                                   ShipPhoneNumber = o.ShipPhoneNumber,
                                   OrderDate = o.OrderDate,
                                   Status = o.Status,
                                   UserId = o.UserId,
                                   Price = od.Price,
                                   ProductId = od.ProductId,
                                   Quantity = od.Quantity,
                               }).FirstOrDefaultAsync();
            if (order == null) throw new ECommerceException("Cannot find the order ");

            return order;
        }
    }
}