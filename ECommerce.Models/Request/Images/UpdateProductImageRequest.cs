using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Request.Images
{
    public class UpdateProductImageRequest
    {
        public bool IsDefault { get; set; }

        [MaxLength(200, ErrorMessage = "Max length of caption is 200 characters")]
        public string Caption { get; set; }

        public int SortOrder { get; set; }
    }
}