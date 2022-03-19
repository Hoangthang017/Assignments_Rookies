namespace ECommerce.Utilities
{
    public static class SystemConstants
    {
        public static class AnonymousAccountSettings
        {
            public static Guid Id = new Guid("00000000-0000-0000-0000-000000000001");
            public static string UserName = "guest";
            public static string Password = "00000000-0000-0000-0000-000000000001";
        }

        public static class AppSettings
        {
            public const string MainConnectionString = "ECommerceDB";
            public const string ErrorResponseSessionKey = "ApiErrorResponse";
            public const string BackendApiAddress = "https://localhost:7195";
            public const string IdentityServerAddress = "https://localhost:5001";
        }

        public static class ImageSettings
        {
            public const string FolderSaveImage = "user-content";
            public const string FolderSaveUserImage = "user";
            public const string FolderSaveProductImage = "product";

            public const string NameOfDefaultAvatart = "user-default.jpg";
            public const string NameOfDefaultProductImage = "default-product-image.png";

            public const int NumberOfSlide = 2;

            public static string DefaultAvatart = Path.Combine(
                AppSettings.BackendApiAddress,
                FolderSaveImage,
                FolderSaveUserImage,
                NameOfDefaultAvatart);

            public static string DefaultProductImage = Path.Combine(
                AppSettings.BackendApiAddress,
                FolderSaveImage,
                FolderSaveProductImage,
                NameOfDefaultProductImage);
        }

        public static class LanguageSettings
        {
            public const string DefaultLanguageId = "en-us";
        }

        public static class ProductSettings
        {
            public const int NumberOfFeaturedProducts = 8;
            public const int NumberOfRelatedProducts = 4;
            public const int pageSizePaging = 12;
        }

        public static class CategorySettings
        {
            public const string CartSessionKey = "card";
            public const int NumberOfFeaturedCategory = 4;
        }
    }
}