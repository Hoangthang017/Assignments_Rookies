using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities
{
    public class CategoryTranslation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        [MaxLength(200)]
        public string Name { set; get; }

        [Required]
        [MaxLength(200)]
        public string SeoTitle { set; get; }

        [Required]
        [MaxLength(200)]
        public string SeoAlias { set; get; }

        [Required]
        [MaxLength(500)]
        public string SeoDescription { set; get; }

        public int CategoryId { set; get; }

        [ForeignKey("CategoryId")]
        public Category Category { set; get; }

        [Required]
        [MaxLength(5)]
        public string LanguageId { set; get; }

        [ForeignKey("LanguageId")]
        public Language Language { set; get; }
    }
}