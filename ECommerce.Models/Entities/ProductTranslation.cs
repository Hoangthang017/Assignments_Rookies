using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Entities
{
    public class ProductTranslation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "ntext")]
        public string Detail { get; set; }

        public int LanguageId { get; set; }

        public Language Language { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}