namespace ECommerce.Models.Request.Orders
{
    public class OrderProductRequest
    {
        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}