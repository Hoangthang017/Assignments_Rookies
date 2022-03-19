namespace ECommerce.Models.Request.Images
{
    public class UpdateUserImageRequest : BaseImageRequest
    {
        public string UserId { get; set; }
    }
}