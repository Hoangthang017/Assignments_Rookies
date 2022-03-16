using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities
{
    public class OrderDetail
    {
        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public int OrderId { set; get; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int ProductId { set; get; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}