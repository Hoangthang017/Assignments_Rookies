using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities
{
    public class ProductTranslation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        [MaxLength(200)]
        public string Name { set; get; }

        [Required]
        [MaxLength(200)]
        public string Description { set; get; }

        [Required]
        [MaxLength(500)]
        public string Details { set; get; }

        [Required]
        [MaxLength(200)]
        public string SeoDescription { set; get; }

        [Required]
        [MaxLength(200)]
        public string SeoTitle { set; get; }

        [Required]
        [MaxLength(200)]
        public string SeoAlias { get; set; }

        public string LanguageId { set; get; }

        [ForeignKey("LanguageId")]
        public Language Language { set; get; }

        public int ProductId { set; get; }

        [ForeignKey("ProductId")]
        public Product Product { set; get; }
    }
}