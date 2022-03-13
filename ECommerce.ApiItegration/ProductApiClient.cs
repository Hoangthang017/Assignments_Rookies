using ECommerce.Models.ViewModels.Products;
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
        public ProductApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory, configuration)
        {
        }

        public async Task<List<ProductViewModel>> GetFeaturedProduct(int categoryId, int take, string languageId)
        {
            return await GetListAsync<ProductViewModel>("api/Products/featured/" + categoryId.ToString() + "/" + take.ToString() + "/" + languageId);
        }
    }
}