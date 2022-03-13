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

//JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
//builder.Services.AddAuthentication();
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = "Cookies";
//    options.DefaultChallengeScheme = "oidc";
//})
//    .AddCookie("Cookies")
//    .AddOpenIdConnect("oidc", options =>
//    {
//        options.Authority = "https://localhost:5001";

//        options.ClientId = "interactive";
//        options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
//        options.ResponseType = "code";

//        options.Scope.Add("openid");
//        options.Scope.Add("profile");
//        options.Scope.Add("scope2");

//        options.SaveTokens = true;
//    });

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
    endpoints.MapDefaultControllerRoute();
});
app.Run();