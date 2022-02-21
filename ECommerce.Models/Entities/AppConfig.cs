using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Entities
{
    public class AppConfig
    {
        [Key]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}