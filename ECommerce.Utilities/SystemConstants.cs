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

        public static class AppSettings
        {
            public const string DefaultLanguageId = "DefaultLanguageId";
            public const string Token = "Token";
            public const string BaseAddress = "BaseAddress";
            public const string BackendApiAddress = "https://localhost:7195";
        }

        public static class ProductSettings
        {
            public const int NumberOfFeaturedProducts = 4;
        }
    }
}