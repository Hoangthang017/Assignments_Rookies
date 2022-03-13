using ECommerce.Models.ViewModels.Categories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ApiItegration
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory, configuration)
        {
        }

        public async Task<List<BaseCategoryViewModel>> GetFeaturedCategory(string languageId, int take)
        {
            return await GetListAsync<BaseCategoryViewModel>("api/categories/featured/" + take.ToString() + "/" + languageId);
        }
    }
}