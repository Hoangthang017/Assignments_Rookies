using ECommerce.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities
{
    public class Promotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        public DateTime FromDate { set; get; }

        public DateTime ToDate { set; get; }

        public bool ApplyForAll { set; get; }

        public int? DiscountPercent { set; get; }

        public decimal? DiscountAmount { set; get; }

        public string ProductIds { set; get; }

        public string ProductCategoryIds { set; get; }

        public Status Status { set; get; }

        [Required]
        public string Name { set; get; }
    }
}