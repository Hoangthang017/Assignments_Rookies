// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using ECommerce.DataAccess.EF;
using ECommerce.Models.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Linq;
using System.Security.Claims;

namespace ECommerce.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ECommerceDbContext>(options =>
               options.UseSqlite(connectionString));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ECommerceDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ECommerceDbContext>();
                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    var admin = userMgr.FindByNameAsync("admin").Result;
                    if (admin == null)
                    {
                        admin = new User
                        {
                            UserName = "admin",
                            Email = "admin@email.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(admin, "admin@123").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(admin, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "admin"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("admin created");
                    }
                    else
                    {
                        Log.Debug("admin already exists");
                    }

                    var client = userMgr.FindByNameAsync("client").Result;
                    if (client == null)
                    {
                        client = new User
                        {
                            UserName = "client",
                            Email = "client@email.com",
                            EmailConfirmed = true
                        };
                        var result = userMgr.CreateAsync(client, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(client, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "client"),
                            new Claim("location", "VN")
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("client created");
                    }
                    else
                    {
                        Log.Debug("client already exists");
                    }
                }
            }
        }
    }
}