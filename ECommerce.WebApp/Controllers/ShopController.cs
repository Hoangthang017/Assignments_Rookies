using ECommerce.ApiItegration;
using ECommerce.Models.Request.Common;
using ECommerce.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductApiClient _productApiClient;

        public ShopController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        // GET: en-us?pageIndex=1&pagSize=1&categoryId=1
        public async Task<IActionResult> Index(string culture, [FromQuery] GetProductPagingRequest request)
        {
            return View(new ShopVM()
            {
                ProductViewModels = await _productApiClient.GetAllPaging(culture, request)
            });
        }
    }
}