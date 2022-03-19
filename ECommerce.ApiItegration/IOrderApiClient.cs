using ECommerce.Models.Request.Orders;
using ECommerce.Models.ViewModels.Orders;

namespace ECommerce.ApiItegration
{
    public interface IOrderApiClient : IBaseApiClinet
    {
        Task<OrderViewModel> CreateOrder(CreateOrderRequest request);
    }
}