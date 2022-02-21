using ECommerce.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        public DateTime OrderDate { set; get; }

        [Required]
        [MaxLength(200)]
        public string ShipName { set; get; }

        [Required]
        [MaxLength(200)]
        public string ShipAddress { set; get; }

        [Required]
        [MaxLength(50)]
        public string ShipEmail { set; get; }

        [Required]
        [MaxLength(200)]
        public string ShipPhoneNumber { set; get; }

        public OrderStatus Status { set; get; }

        public IEnumerable<OrderDetail> OrderDetails { set; get; }

        public Guid UserId { set; get; }

        [ForeignKey("UserId")]
        public User USer { get; set; }
    }
}