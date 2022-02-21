using ECommerce.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        [MaxLength(200)]
        public string Name { set; get; }

        [Required]
        [MaxLength(50)]
        public string Email { set; get; }

        [Required]
        [MaxLength(200)]
        public string PhoneNumber { set; get; }

        [Required]
        public string Message { set; get; }

        public Status Status { set; get; }
    }
}