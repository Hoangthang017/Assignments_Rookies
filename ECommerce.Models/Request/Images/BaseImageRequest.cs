using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Request.Images
{
    public class BaseImageRequest
    {
        [MaxLength(200, ErrorMessage = "Max length of caption is 200 characters")]
        public string Caption { get; set; }

        public int SortOrder { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}