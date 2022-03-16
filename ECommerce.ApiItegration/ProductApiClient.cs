using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Products;
using ECommerce.Models.ViewModels.Common;
using ECommerce.Models.ViewModels.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ApiItegration
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        private readonly string BaseUrlApi = "api/products";

        private readonly Dictionary<string, object> productReviewRequest = new Dictionary<string, object>() { };

        public ProductApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        // POST: api/products/1/review/asdasd-adasd
        public async Task<ProductReviewViewModel> CreateProductReview(int productId, string customerId, CreateProductReviewRequest request)
        {
            productReviewRequest.Add("rating", request.Rating);
            productReviewRequest.Add("comment", request.Comment);
            return await PostAsync<ProductReviewViewModel>(Path.Combine(BaseUrlApi, productId.ToString(), "review", customerId), productReviewRequest);
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
    }
}