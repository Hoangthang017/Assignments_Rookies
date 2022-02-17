using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal OriginalPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        public int ViewCount { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        public IEnumerable<ProductImage> ProductImages { get; set; }

        public IEnumerable<ProductTranslation> ProductTranslations { get; set; }

        public IEnumerable<ProductInCategory> ProductInCategories { get; set; }
    }
}