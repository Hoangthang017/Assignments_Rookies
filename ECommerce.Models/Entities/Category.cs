using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool IsShowOnHome { get; set; }

        public int? ParentId { get; set; }

        [Required]
        public int Status { get; set; }

        public IEnumerable<ProductInCategory> ProductInCategories { get; set; }

        public IEnumerable<CategoryTranslation> CategoryTranslations { get; set; }
    }
}