using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Products;
using ECommerce.Models.ViewModels.Common;
using ECommerce.Models.ViewModels.Products;
using ECommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ECommerce.ApiItegration
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        private readonly string BaseUrlApi = "api/products";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        // POST: api/products/1/review/asdasd-adasd
        public async Task<ProductReviewViewModel> CreateProductReview(int productId, string customerId, CreateProductReviewRequest request)
        {
            return await PostAsync<ProductReviewViewModel, CreateProductReviewRequest>(Path.Combine(BaseUrlApi, productId.ToString(), "review", customerId), request);
        }

        //paging/en-us?pageIndex=1&pagSize=1&categoryId=1
        public async Task<PageResult<ProductViewModel>> GetAllPaging(string languageId, GetProductPagingRequest request)
        {
            var data = await GetAsync<PageResult<ProductViewModel>>(
                $"/api/products/paging/{languageId}?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&categoryId={request.CategoryId}");

            return data;
        }

        public async Task<List<ProductReviewViewModel>> GetAllReview(int productId)
        {
            return await GetListAsync<ProductReviewViewModel>(Path.Combine(BaseUrlApi, productId.ToString(), "review"));
        }

        public async Task<List<ProductViewModel>> GetFeaturedProduct(int categoryId, int take, string languageId)
        {
            return await GetListAsync<ProductViewModel>(Path.Combine(BaseUrlApi, "featured", categoryId.ToString(), take.ToString(), languageId));
        }

        public async Task<ProductViewModel> GetProductById(string languageId, int productId)
        {
            return await GetAsync<ProductViewModel>(Path.Combine(BaseUrlApi, languageId, productId.ToString()));
        }

        public async Task<List<ProductViewModel>> GetRelatedProduct(string languageId, int productId, int categoryId, int take)
        {
            return await GetListAsync<ProductViewModel>(Path.Combine(BaseUrlApi, "related", languageId, productId.ToString(), categoryId.ToString(), take.ToString()));
        }

        public async Task<bool> UpdateViewCount(int productId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(SystemConstants.AppSettings.BackendApiAddress);
            var sessions = _httpContextAccessor
                    .HttpContext
                    .Session
                    .GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var httpContent = new StringContent("", Encoding.UTF8, "application/json");

            var response = await client.PatchAsync(Path.Combine(BaseUrlApi, "viewcount", productId.ToString()), httpContent);

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return true;
            };

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                _httpContextAccessor.HttpContext.Session.SetString(SystemConstants.AppSettings.ErrorResponseSessionKey, body);
            }

            return false;
        }
    }
}