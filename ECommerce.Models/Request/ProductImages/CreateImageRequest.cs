using Microsoft.AspNetCore.Http;

namespace ECommerce.Models.Request.ProductImages
{
    public class CreateImageRequest
    {
        public string Caption { get; set; }
        public bool IsDefault { get; set; }
        public int SortOrder { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}