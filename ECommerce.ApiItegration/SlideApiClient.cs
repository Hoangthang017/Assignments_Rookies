using ECommerce.Models.ViewModels.Slides;
using Microsoft.Extensions.Configuration;

namespace ECommerce.ApiItegration
{
    public class SlideApiClient : BaseApiClient, ISlideApiClient
    {
        public SlideApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory, configuration)
        {
        }

        public async Task<List<SlideViewModel>> GetAllSlide(int take)
        {
            return await GetListAsync<SlideViewModel>("/api/images/slide/" + take);
        }
    }
}