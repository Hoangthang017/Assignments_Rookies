﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
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
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope1"),
                new ApiScope("scope2"),
                new ApiScope("swaggerApi", "Swagger API"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                 // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "admin",
                    ClientSecrets = { new Secret("CC55E8B8-4A32-48D4-8853-6EDB1CD2EBA1".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    AllowedCorsOrigins = { "https://localhost:7004" },

                    RedirectUris = { "https://localhost:7004/" },
                    FrontChannelLogoutUri = "https://localhost:7004/",
                    PostLogoutRedirectUris = { "https://localhost:7004/signout-callback-oidc" },

                    AllowOfflineAccess = true,

                    // no consent page
                    RequireConsent = false,

                    AllowedScopes = { "openid", "profile", "swaggerApi" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:7158/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:7158/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:7158/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "scope2" }
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
                    AllowedScopes = { "openid", "profile", "swaggerApi" }
                },
            };
    }
}