using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities
{
    public class ProductTag
    {
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public string TagId { get; set; }

        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }
    }
}