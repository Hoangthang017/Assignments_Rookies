using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Entities
{
    public class Language
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        public IEnumerable<ProductTranslation> ProductTranslations { get; set; }

        public IEnumerable<CategoryTranslation> CategoryTranslations { get; set; }
    }
}