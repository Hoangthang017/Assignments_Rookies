using ECommerce.Models.ViewModels.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ECommerce.ApiItegration
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        private readonly string BaseApiUrl = "api/categories";

        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<List<BaseCategoryViewModel>> GetActiveCategory(string languageId)
        {
            return await GetListAsync<BaseCategoryViewModel>(Path.Combine(BaseApiUrl, "active", languageId));
        }

        public async Task<List<BaseCategoryViewModel>> GetFeaturedCategory(string languageId, int take)
        {
            return await GetListAsync<BaseCategoryViewModel>(Path.Combine(BaseApiUrl, "featured", take.ToString(), languageId));
        }
    }
}