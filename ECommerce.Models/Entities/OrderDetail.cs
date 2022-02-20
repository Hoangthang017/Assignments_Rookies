using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities
{
    public class OrderDetail
    {
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
    }
}