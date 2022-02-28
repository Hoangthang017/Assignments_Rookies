using IdentityServer4;
using IdentityServer4.Models;

namespace ECommerce.Models.IdentityServer
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> Ids =>
          new IdentityResource[]
          {
              // user quản lý nhứng gì này // chuẩn của identity ít nhất phải có 2 thằng này
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
              //vd:
              // new IdentityResources.Email(),
              // new IdentityResources.Phone()
          };

        // danh sách các Api ở đây ta chỉ có mỗi thằng knowledgespace
        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("api.BackendApi", "Backend API")
            };

        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
                new ApiScope("api.BackendApi", "Backend API")
        };

        /*  định nghĩa ra các Client chín là các ứng dụng ta định làm ,
         *  chính là webportal , server (chính là swagger) và .. */

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "BackendApi",
                    ClientSecrets = { new Secret("BackendApi_Secret".Sha256()) },//  mã hóa theo Sha256

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequireConsent = false,
                    RequirePkce = true,
                    AllowAccessTokensViaBrowser = false,
                    AllowedCorsOrigins={"https://localhost:7195" },

                    // ở client này cho phép chuy cập đến những cái này
                    AllowedScopes = new List<string>
                    {
                        // ở đây chúng ta cho chuy cập cả thông tin user lần api
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api.BackendApi"
                    }
                 },
                new Client
                {
                    ClientName = "Swagger Client",
                    ClientId = "swagger",
                    ClientSecrets = { new Secret("swagger_Secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedCorsOrigins = { "https://localhost:7195" }, // cho phép nguồn gốc cores

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api.BackendApi"
                    }
                },
            };
    }
}