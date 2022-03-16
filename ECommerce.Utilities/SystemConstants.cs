using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Utilities
{
    public static class SystemConstants
    {
        public const string MainConnectionString = "ECommerceDB";
        public const string CartSessionKey = "card";

        public static class AnonymousAccountSettings
        {
            public static Guid Id = new Guid("00000000-0000-0000-0000-000000000001");
            public static string UserName = "guest";
            public static string Password = "00000000-0000-0000-0000-000000000001";
        }

        public static class AppSettings
        {
            public const string BackendApiAddress = "https://localhost:7195";
            public const string IdentityServerAddress = "https://localhost:5001";
            public const string DefaultAvatart = "https://localhost:7195/user-content/user/user-default.jpg";
        }

        public static class SlideSettings
        {
            public const int NumberOfSlide = 2;
        }

        public static class LanguageSettings
        {
            public const string DefaultLanguageId = "en-us";
        }

        public static class ProductSettings
        {
            public const int NumberOfFeaturedProducts = 8;
            public const int NumberOfRelatedProducts = 4;

            public const int pageSizePaging = 4;
        }

        public static class CategorySettings
        {
            public const int NumberOfFeaturedCategory = 4;
        }
    }
}