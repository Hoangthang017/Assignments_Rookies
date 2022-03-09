using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Entities
{
    public class ProductImage
    {
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int ImageId { get; set; }

        [ForeignKey("ImageId")]
        public Image Image { get; set; }

        public bool IsDefault { get; set; }
    }
}