using ECommerce.ApiItegration;
using ECommerce.Models.ViewModels.Slides;
using ECommerce.Utilities;
using ECommerce.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerce.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;

        public HomeController(
            ILogger<HomeController> logger,
            ISlideApiClient slideApiClient,
            IProductApiClient productApiClient,
            ICategoryApiClient categoryApiClient)
        {
            _logger = logger;
            _slideApiClient = slideApiClient;
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Index([FromQuery] int? categoryId, string culture)
        {
            var viewModel = new HomeViewModel
            {
                FeaturedProducts = await _productApiClient.GetFeaturedProduct(
                    categoryId != null ? (int)categoryId : 0,
                    SystemConstants.ProductSettings.NumberOfFeaturedProducts,
                    String.IsNullOrEmpty(culture) ? SystemConstants.LanguageSettings.DefaultLanguageId : culture)
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}