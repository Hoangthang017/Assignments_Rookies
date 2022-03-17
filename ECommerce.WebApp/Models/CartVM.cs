using ECommerce.Models.Request.Orders;
using ECommerce.Models.ViewModels.Products;

namespace ECommerce.WebApp.Models
{
    public class CartVM
    {
        public ProductViewModel Product { get; set; }

        public int Quantity { get; set; }

        public decimal GetTotalPrice()
        {
            return Quantity * Product.Price;
        }
    }
}