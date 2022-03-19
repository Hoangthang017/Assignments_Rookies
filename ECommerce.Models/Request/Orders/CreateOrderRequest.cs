using ECommerce.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Request.Orders
{
    public class CreateOrderRequest
    {
        [Required(ErrorMessage = "Ship name is required")]
        [MaxLength(200, ErrorMessage = "Maximum length of ship name is 200 characters")]
        public string ShipName { set; get; }

        [Required(ErrorMessage = "Ship address is required")]
        [MaxLength(200, ErrorMessage = "Maximum length of ship address is 200 characters")]
        public string ShipAddress { set; get; }

        [Required(ErrorMessage = "Ship email is required")]
        [MaxLength(200, ErrorMessage = "Maximum length of ship email is 50 characters")]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$",
            ErrorMessage = "Email is not match")]
        public string ShipEmail { set; get; }

        [RegularExpression(@"^[0-9\-\+]{9,15}$",
            ErrorMessage = "Phone number is not match")]
        public string ShipPhoneNumber { set; get; }

        public OrderStatus Status { set; get; }

        public List<OrderProductRequest>? OrderProduct { get; set; }
    }
}