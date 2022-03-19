using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Request.Categories
{
    public class BaseCategoryRequest
    {
        [Required(ErrorMessage = "Seo name is required")]
        [MaxLength(200, ErrorMessage = "The maximum length of Seo name is 200 characters")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Seo title is required")]
        [MaxLength(200, ErrorMessage = "The maximum length of Seo title is 200 characters")]
        public string SeoTitle { set; get; }

        [Required(ErrorMessage = "Seo alias is required")]
        [MaxLength(200, ErrorMessage = "The maximum length of Seo alias is 200 characters")]
        public string SeoAlias { set; get; }

        [Required(ErrorMessage = "Seo description is required")]
        [MaxLength(500, ErrorMessage = "The maximum length of Seo description is 500 characters")]
        public string SeoDescription { set; get; }
    }
}