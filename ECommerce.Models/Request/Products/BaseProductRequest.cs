using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Request.Products
{
    public class BaseProductRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200, ErrorMessage = "Max length of Name is 200 characters")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(200, ErrorMessage = "Max length of Description is 200 characters")]
        public string Description { set; get; }

        [Required(ErrorMessage = "Detail is required")]
        [MaxLength(500, ErrorMessage = "Max length of Detail is 500 characters")]
        public string Details { set; get; }

        [Required(ErrorMessage = "Seo description is required")]
        [MaxLength(200, ErrorMessage = "Max length of seo description is 200 characters")]
        public string SeoDescription { set; get; }

        [Required(ErrorMessage = "Seo title is required")]
        [MaxLength(200, ErrorMessage = "Max length of seo title is 200 characters")]
        public string SeoTitle { set; get; }

        [Required(ErrorMessage = "Seo alias is required")]
        [MaxLength(200, ErrorMessage = "Max length of seo alias is 200 characters")]
        public string SeoAlias { get; set; }

        public bool IsShowOnHome { get; set; }
    }
}