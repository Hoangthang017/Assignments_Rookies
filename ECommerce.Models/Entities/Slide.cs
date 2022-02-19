using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities
{
    public class Slide
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        [MaxLength(256)]
        public string Image { get; set; }

        [MaxLength(256)]
        public string Url { get; set; }

        public int? DisplayOrder { get; set; }

        public bool Status { get; set; }
    }
}