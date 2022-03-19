using ECommerce.Models.Request.Orders;
using ECommerce.Models.ViewModels.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ECommerce.ApiItegration
{
    public class OrderApiClient : BaseApiClient, IOrderApiClient
    {
        private readonly string baseBackendUrl = "api/orders";

        public OrderApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<OrderViewModel> CreateOrder(CreateOrderRequest request)
        {
            return await PostAsync<OrderViewModel, CreateOrderRequest>(Path.Combine(baseBackendUrl), request, true);
        }
    }
}