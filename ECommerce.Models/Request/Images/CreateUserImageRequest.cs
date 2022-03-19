namespace ECommerce.Models.Request.Images
{
    public class CreateUserImageRequest : BaseImageRequest
    {
        public string UserId { get; set; }
    }
}