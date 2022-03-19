using ECommerce.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Request.Categories
{
    public class CreateCategoryRequest
    {
        public int SortOrder { set; get; }

        public bool IsShowOnHome { set; get; }

        public int? ParentId { set; get; }

        public Status Status { set; get; }

        [Required(ErrorMessage = "LanguageId is required")]
        [MaxLength(5, ErrorMessage = "Max length of languageId is 5 characters")]
        public string languageId { get; set; }
    }
}