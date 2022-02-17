using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Entities
{
    public class CategoryTranslation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int LanguageId { get; set; }

        public Language Language { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}