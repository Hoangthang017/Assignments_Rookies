using ECommerce.ApiItegration;
using ECommerce.Models.Request.Products;
using ECommerce.Utilities;
using ECommerce.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ECommerce.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductController(IProductApiClient productApiClient, IHttpContextAccessor httpContextAccessor)
        {
            _productApiClient = productApiClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Detail(string culture, int productId)
        {
            var productVM = await _productApiClient.GetProductById(culture, productId);
            await _productApiClient.UpdateViewCount(productId);
            return View(new ProductVM()
            {
                ProductViewModel = productVM,
                ProductReviewViewModels = await _productApiClient.GetAllReview(productId),
                RelatedProducts = await _productApiClient.GetRelatedProduct(
                    culture, productId, productVM.categoryId == null ? 0 : productVM.categoryId, SystemConstants.ProductSettings.NumberOfRelatedProducts)
            });
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> Rating(int productId, CreateProductReviewRequest request)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            if (tokenS != null)
            {
                var customerId = tokenS.Claims.FirstOrDefault(x => x.Type == "sub").Value;
                var reviewVM = await _productApiClient.CreateProductReview(productId, customerId, request);
            }
            return RedirectToAction(actionName: "detail", controllerName: "product", routeValues: new { productId = productId });
        }
    }
}