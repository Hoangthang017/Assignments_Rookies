using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Entities
{
    public class VisitorStatistic
    {
        [Key]
        public Guid Id { set; get; }

        [Required]
        public DateTime VisitedDate { set; get; }

        [MaxLength(50)]
        public string IpAddress { set; get; }
    }
}