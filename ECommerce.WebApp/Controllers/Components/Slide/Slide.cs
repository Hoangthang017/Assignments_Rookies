using ECommerce.ApiItegration;
using ECommerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApp.Controllers.Components.Slide
{
    [ViewComponent]
    public class Slide : ViewComponent
    {
        private readonly ISlideApiClient _slideApiClient;

        public Slide(ISlideApiClient slideApiClient)
        {
            _slideApiClient = slideApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _slideApiClient.GetAllSlide(SystemConstants.SlideSettings.NumberOfSlide));
        }
    }
}