using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.DataAccess.Infrastructure.Common;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.AutoMapper;
using ECommerce.Models.Entities;
using ECommerce.Models.IdentityServer;
using ECommerce.Models.Request.Validations;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// add json handler & fluent validation
builder.Services.AddControllers()
    .AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

// add dbcontext
builder.Services.AddDbContext<ECommerceDbContext>(options =>
    {
        string connectstring = builder.Configuration.GetConnectionString("ECommerceDB");
        options.UseSqlServer(connectstring);
    });

// DI services
// add unit of work pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// add file storage for handler image
builder.Services.AddTransient<IStorageService, FileStorageService>();

// Identity Server 4
// add identity EF
builder.Services.AddIdentity<User, Role>() // use custome user and role
            .AddEntityFrameworkStores<ECommerceDbContext>() // declare DbContext
                                                            // add default token provider used to generate tokens for reset password,
                                                            // change mail and telephone number, operations and for 2FA token generation
            .AddDefaultTokenProviders();

// add identity server
builder.Services.AddIdentityServer(options => // custome event for identity server
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
})
    .AddInMemoryApiResources(IdentityServerConfig.Apis) // using in-memory resources
    .AddInMemoryClients(IdentityServerConfig.Clients)
    .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
    .AddAspNetIdentity<User>() // declare user using identity server
    .AddDeveloperSigningCredential();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["AuthorityUrl"];
    options.TokenValidationParameters.ValidateAudience = false;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
    builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
    //setup.AddDefaultPolicy(policy =>
    //{
    //    policy.AllowAnyHeader();
    //    policy.AllowAnyMethod();
    //    policy.WithOrigins("https://localhost:5001/api/authenticate", "https://localhost:44401");
    //    policy.AllowCredentials();
    //});
});

// add author
builder.Services.AddAuthorization();

// add swagger
builder.Services.AddSwaggerGen(c =>
{
    // add swagger ui
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Description = "Swagger UI for ECommerce Web",
        Title = "Swagger Ecommerce ",
        Version = "1.0.0"
    });

    // add identity server to swagger
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = @"Sign In to get authorize",
        Name = "Authorization",
        Type = SecuritySchemeType.OAuth2,
        Scheme = "oauth2",
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri(builder.Configuration["AuthorityUrl"] + "/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    //{ "swaggerApi", "Swagger API" }
                },
            }
        }
    });

    // add identity server to swagger
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
             },
             new List<string>{ "swaggerApi" }
        }
    });
});

// add auto mapper
builder.Services.AddAutoMapper(mc =>
{
    mc.AddProfile(new ECommerceMapperProfile());
});

// build app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("CorsPolicy");

// security
app.UseIdentityServer();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

// swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Ecommerce v1");
    c.OAuthClientId("swagger");
    c.OAuthScopeSeparator(" ");
    c.OAuthClientSecret("3EEF0E6D-F9D8-496D-B53D-64C253FCD6EE");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();