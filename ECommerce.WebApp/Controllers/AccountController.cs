using ECommerce.ApiItegration;
using ECommerce.Models.Request.Users;
using ECommerce.Models.ViewModels.UserInfos;
using ECommerce.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserApiClient _userApiClient;

        public AccountController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Authenticate(LoginRequest request)
        {
            var response = await _userApiClient.Authenticate(request);
            if (response.Count == 0)
            {
                TempData["loginErrorMessage"] = "User name or Password is incorrect!!!";
                return RedirectToAction(actionName: "index", controllerName: "account");
            }
            HttpContext.Session.SetString("token", response["token"]);
            return RedirectToAction(actionName: "index", controllerName: "home");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("token");
            await _userApiClient.RevokeToken();
            return RedirectToAction(actionName: "index", controllerName: "home");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (ModelState.IsValid)
            {
                var response = await _userApiClient.Register(register.UserName, register.Password);
                if (response != null)
                {
                    await Authenticate(new LoginRequest()
                    {
                        UserName = register.UserName,
                        Password = register.Password
                    });
                    return RedirectToAction(actionName: "index", controllerName: "home");
                }
            }
            return View();
        }
    }
}