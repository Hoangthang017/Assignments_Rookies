// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Models;
using System.Collections.Generic;

namespace ECommerce.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
               new IdentityResource[]
               {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email(),
                    new IdentityResources.Phone(),

                    // customize claims
                    new IdentityResource("userInfor", new[]{ "role", "displayName", "firstName","lastName", "dateOfBirth"})
                };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("swaggerApi")
                {
                    UserClaims =
                    {
                    }
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                 // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "react-admin",
                    ClientSecrets = { new Secret("D013F030-0177-4F0D-AECA-1206D0608408".Sha256() )},

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,

                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { "https://localhost:3000/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:3000/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:3000/signout-callback-oidc" },

                    AllowedCorsOrigins = new[] { "https://localhost:3000"},

                    AllowOfflineAccess = true,

                    AllowedScopes = {
                        "openid",
                        "profile",
                        "swaggerApi",
                        "userInfor",
                        "email",
                        "phone"
                    }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "web-app",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,

                    AllowedCorsOrigins = { "https://localhost:7158" },
                    RedirectUris = { "https://localhost:7158/en-us/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:7158/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:7158/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "phone", "email" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "swagger",
                    ClientSecrets = { new Secret("3EEF0E6D-F9D8-496D-B53D-64C253FCD6EE".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AllowedCorsOrigins = { "https://localhost:7195" },

                    RedirectUris = { "https://localhost:7159/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:7159/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:7159/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        "openid",
                        "profile",
                        "swaggerApi" ,
                        "userInfor",
                        "email",
                        "phone" }
                },
            };
    }
}