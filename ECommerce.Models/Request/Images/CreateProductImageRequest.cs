namespace ECommerce.Models.Request.Images
{
    public class CreateProductImageRequest : BaseImageRequest
    {
        public bool IsDefault { get; set; }

        public int ProductId { get; set; }
    }
}