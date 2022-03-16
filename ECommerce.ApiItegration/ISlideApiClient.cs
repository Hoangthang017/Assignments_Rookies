using ECommerce.Models.ViewModels.Slides;

namespace ECommerce.ApiItegration
{
    public interface ISlideApiClient
    {
        Task<List<SlideViewModel>> GetAllSlide(int take);
    }
}