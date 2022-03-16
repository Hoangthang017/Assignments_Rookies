using ECommerce.Models.Enums;
using ECommerce.Models.Request.Orders;

namespace ECommerce.WebApp.Models
{
    public class CheckoutVM
    {
        public string ShipName { set; get; }

        public string ShipAddress { set; get; }

        public string ShipEmail { set; get; }

        public string ShipPhoneNumber { set; get; }

        public OrderStatus Status { set; get; }

        public List<BaseProductOrderRequest> ProductOrderRequests { set; get; }

        public CheckoutVM()
        {
            ProductOrderRequests = new List<BaseProductOrderRequest>();
        }
    }
}