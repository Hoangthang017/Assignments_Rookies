using ECommerce.ApiItegration;
using ECommerce.Models.Request.Common;
using ECommerce.Utilities;
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
            // check hand initial default value for request
            if (request.PageIndex == 0) request.PageIndex = 1;
            if (request.PageSize == 0) request.PageSize = SystemConstants.ProductSettings.pageSizePaging;

            return View(new ShopVM()
            {
                ProductViewModels = await _productApiClient.GetAllPaging(culture, request)
            });
        }
    }
}