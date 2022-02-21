using ECommerce.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities
{
    public class ProductReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime ReviewDate { get; set; }

        [Required]
        public Rating Rating { get; set; }

        [Required]
        [MaxLength(500)]
        public string Comment { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public Guid CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public User Customer { get; set; }
    }
}