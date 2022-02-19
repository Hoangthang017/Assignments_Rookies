namespace ECommerce.Models.Abstract
{
    public interface IAuditAble
    {
        // entity
        DateTime? CreatedDate { get; set; }

        string CreatedBy { get; set; }

        DateTime? UpdatedDate { get; set; }

        string UpdatedBy { get; set; }

        // seo
        string MeteKeyword { get; set; }

        string MetaDescription { get; set; }

        // state
        bool Status { get; set; }
    }
}