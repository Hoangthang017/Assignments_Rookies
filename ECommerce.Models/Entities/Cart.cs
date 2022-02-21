using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public DateTime DateCreated { get; set; }

        public int ProductId { set; get; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}