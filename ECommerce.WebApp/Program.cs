using ECommerce.ApiItegration;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddTransient<ISlideApiClient, SlideApiClient>();
builder.Services.AddTransient<IProductApiClient, ProductApiClient>();
builder.Services.AddTransient<ICategoryApiClient, CategoryApiClient>();
builder.Services.AddTransient<IUserApiClient, UserApiClient>();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services.AddAuthentication();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:5001";

        options.ClientId = "WebApp";
        options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
        options.ResponseType = "code";

        options.Scope.Add("openid");
        options.Scope.Add("profile");

        options.SaveTokens = true;
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
//.UseAuthentication();

app.UseAuthorization();
app.UseSession();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "acoount Index en-us",
        pattern: "login", new
        {
            controller = "account",
            action = "Index"
        });
    endpoints.MapControllerRoute(
       name: "acount register en-us",
       pattern: "register", new
       {
           controller = "account",
           action = "register"
       });
    endpoints.MapControllerRoute(
        name: "Product Detail en-us",
        pattern: "{culture=en-us}/product/{productId?}", new
        {
            controller = "Product",
            action = "Detail"
        });
    endpoints.MapControllerRoute(
       name: "product rating en-us",
       pattern: "{culture=en-us}/product/{productId=0}/rating", new
       {
           controller = "product",
           action = "rating"
       });
    endpoints.MapControllerRoute(
        name: "Shop Index en-us",
        pattern: "{culture=en-us}/shop", new
        {
            controller = "Shop",
            action = "Index"
        });
    endpoints.MapControllerRoute(
        name: "Product Detail en-us",
        pattern: "{culture=en-us}/cart", new
        {
            controller = "Cart",
            action = "Cart"
        });
    endpoints.MapControllerRoute(
        name: "Checkout Index en-us",
        pattern: "{culture=en-us}/checkout", new
        {
            controller = "checkout",
            action = "Index"
        });
    endpoints.MapControllerRoute(
        name: "Home Index en-us",
        pattern: "{culture=en-us}", new
        {
            controller = "Home",
            action = "Index"
        });
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{culture=en-us}/{controller=Home}/{action=Index}"
    );
});
app.Run();