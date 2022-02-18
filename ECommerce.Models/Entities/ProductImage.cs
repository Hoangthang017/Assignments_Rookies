using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Entities
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public string Caption { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        [Required]
        public long FileSize { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}