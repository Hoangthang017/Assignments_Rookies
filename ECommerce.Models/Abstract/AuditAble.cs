using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Abstract
{
    public abstract class AuditAble : IAuditAble
    {
        public DateTime? CreatedDate { get; set; }

        [MaxLength(256)]
        public string CreatedBy { get; set; }

        [MaxLength(256)]
        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        [MaxLength(256)]
        public string MeteKeyword { get; set; }

        [MaxLength(256)]
        public string MetaDescription { get; set; }

        public bool Status { get; set; }
    }
}