using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Entities
{
    public class Footer
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }

        [Required]
        public string Content { get; set; }
    }
}