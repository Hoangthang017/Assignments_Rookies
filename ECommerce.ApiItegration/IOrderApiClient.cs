using ECommerce.Models.Request.Orders;
using ECommerce.Models.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ApiItegration
{
    public interface IOrderApiClient : IBaseApiClinet
    {
        Task<OrderViewModel> CreateOrder(CreateOrderRequest request);
    }
}