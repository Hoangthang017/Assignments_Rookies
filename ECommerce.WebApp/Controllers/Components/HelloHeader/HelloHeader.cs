using ECommerce.ApiItegration;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApp.Controllers.Components.HelloHeader
{
    [ViewComponent]
    public class HelloHeader : ViewComponent
    {
        private readonly IUserApiClient _userApiClient;

        public HelloHeader(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userInforVM = await _userApiClient.GetAccountInfor();

            return View(userInforVM);
        }
    }
}