using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Orders;
using ECommerce.Models.ViewModels.Orders;

namespace ECommerce.DataAccess.Repository.OrderRepo
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<int> Create(Guid customerId, CreateOrderRequest request);

        Task<OrderViewModel> GetById(Guid customerId, int orderId);
    }
}