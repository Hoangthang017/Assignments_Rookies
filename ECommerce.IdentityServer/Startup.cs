// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using ECommerce.ApiItegration;
using ECommerce.DataAccess.EF;
using ECommerce.Models.Entities;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ECommerce.IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSingleton<ICorsPolicyService>((container) =>
            {
                var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
                return new DefaultCorsPolicyService(logger)
                {
                    AllowedOrigins = { "https://localhost:44401" }
                };
            });
            services.AddDbContext<ECommerceDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ECommerceDB")));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ECommerceDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IProfileService, ProfileService>();

            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<User>()
                .AddProfileService<ProfileService>();
            services.AddTransient<ISlideApiClient, SlideApiClient>();
            services.AddTransient<IProductApiClient, ProductApiClient>();
            services.AddTransient<ICategoryApiClient, CategoryApiClient>();

            //services.AddAuthentication()
            //    .AddGoogle(options =>
            //    {
            //        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            //        // register your IdentityServer with Google at https://console.developers.google.com
            //        // enable the Google+ API
            //        // set the redirect URI to https://localhost:5001/signin-google
            //        options.ClientId = "copy client ID from Google here";
            //        options.ClientSecret = "copy client secret from Google here";
            //    });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}