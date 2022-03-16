namespace ECommerce.WebApp.Models
{
    public class CartTotalVM
    {
        public decimal SubTotal { get; set; }
        public decimal Delivery { get; set; }
        public decimal Discount { get; set; }
    }
}