using ECommerce.Models.Enums;
using ECommerce.Models.Request.Orders;

namespace ECommerce.Models.ViewModels.Orders
{
    public class OrderViewModel
    {
        public int Id { set; get; }

        public DateTime OrderDate { set; get; }

        public string ShipName { set; get; }

        public string ShipAddress { set; get; }

        public string ShipEmail { set; get; }

        public string ShipPhoneNumber { set; get; }

        public OrderStatus Status { set; get; }

        public Guid UserId { set; get; }

        public List<OrderProductRequest> OrderProducts { get; set; }
    }
}