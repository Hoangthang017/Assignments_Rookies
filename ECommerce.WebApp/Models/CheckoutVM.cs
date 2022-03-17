using ECommerce.Models.Enums;
using ECommerce.Models.Request.Orders;

namespace ECommerce.WebApp.Models
{
    public class CheckoutVM : CreateOrderRequest
    {
        public int? OrderId { get; set; }
    }
}