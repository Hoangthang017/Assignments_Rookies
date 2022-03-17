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

        public async Task<int> Create(Guid customerId, CreateOrderRequest request)
        {
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
                User = customer
            };

            // add list product
            var orderDetails = new List<OrderDetail>();
            foreach (var orderProduct in request.OrderProduct)
            {
                var product = await _context.Products.FindAsync(orderProduct.ProductId);
                if (product == null) throw new ECommerceException($"Cannot find the product has id: {orderProduct.ProductId}");

                orderDetails.Add(new OrderDetail()
                {
                    Order = order,
                    OrderId = order.Id,
                    ProductId = orderProduct.ProductId,
                    Product = product,
                    Price = orderProduct.Price,
                    Quantity = orderProduct.Quantity
                });
            }

            await _context.Orders.AddAsync(order);
            await _context.OrderDetails.AddRangeAsync(orderDetails);
            await _context.SaveChangesAsync();

            return order.Id;
        }

        public async Task<OrderViewModel> GetById(Guid customerId, int orderId)
        {
            // handle anonymous customer
            if (customerId == Guid.Empty) customerId = SystemConstants.AnonymousAccountSettings.Id;

            // get order view model
            var orderVM = await _context.Orders
                    .Where(x => x.Id == orderId && x.UserId == customerId)
                    .Select(x => new OrderViewModel()
                    {
                        Id = x.Id,
                        ShipName = x.ShipName,
                        ShipAddress = x.ShipAddress,
                        ShipEmail = x.ShipEmail,
                        ShipPhoneNumber = x.ShipPhoneNumber,
                        OrderDate = x.OrderDate,
                        Status = x.Status,
                        UserId = x.UserId,
                    }).FirstOrDefaultAsync();
            if (orderVM == null) throw new ECommerceException("Cannot find the order");

            // get all list order details
            var orderDetails = await _context.OrderDetails
                .Where(x => x.OrderId == orderVM.Id)
                .Select(x => new OrderProductRequest()
                {
                    ProductId = x.ProductId,
                    Price = x.Price,
                    Quantity = x.Quantity
                })
                .ToListAsync();
            orderVM.OrderProducts = new List<OrderProductRequest>(orderDetails);

            return orderVM;
        }
    }
}