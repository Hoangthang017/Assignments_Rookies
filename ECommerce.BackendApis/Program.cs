using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// add dbcontext
builder.Services.AddDbContext<ECommerceDbContext>(options =>
{
    string connectstring = builder.Configuration.GetConnectionString("ECommerceDB");
    options.UseSqlServer(connectstring);
});

// add DI services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// add swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Swagger Ecommerce ",
        Version = "v1"
    });
});
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// add auto mapper

builder.Services.AddAutoMapper(mc =>
{
    mc.AddProfile(new ECommerceMapperProfile());
});

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

app.UseRouting();

// security
app.UseAuthorization();

// swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Ecommerce v1");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();