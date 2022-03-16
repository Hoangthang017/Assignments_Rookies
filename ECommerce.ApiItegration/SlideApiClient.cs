using ECommerce.Models.ViewModels.Slides;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ECommerce.ApiItegration
{
    public class SlideApiClient : BaseApiClient, ISlideApiClient
    {
        public SlideApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<List<SlideViewModel>> GetAllSlide(int take)
        {
            return await GetListAsync<SlideViewModel>("/api/images/slide/" + take);
        }
    }
}