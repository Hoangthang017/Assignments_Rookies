using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Entities
{
    public class Language
    {
        [Key]
        [MaxLength(5)]
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public IEnumerable<ProductTranslation> ProductTranslations { get; set; }

        public IEnumerable<CategoryTranslation> CatogeryTranslations { get; set; }
    }
}