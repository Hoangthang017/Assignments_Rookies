using ECommerce.ApiItegration;
using ECommerce.Models.Request.Users;
using ECommerce.Utilities;
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

        // view of authenticate
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //POST: /login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var response = await _userApiClient.Authenticate(request);
            if (response.Count == 0)
            {
                TempData["loginErrorMessage"] = HttpContext.Session.GetString(SystemConstants.AppSettings.ErrorResponseSessionKey);
                return View();
            }
            HttpContext.Session.SetString("token", response["token"]);
            return RedirectToAction(actionName: "index", controllerName: "home");
        }

        //GET: /logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction(actionName: "index", controllerName: "home");
        }

        // view of register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //POST: /register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var response = await _userApiClient.Register(register.UserName, register.Password);
            if (response == null)
            {
                TempData["registerErrorMessage"] = HttpContext.Session.GetString(SystemConstants.AppSettings.ErrorResponseSessionKey);
                return View();
            }
            await Login(new LoginRequest()
            {
                UserName = register.UserName,
                Password = register.Password
            });
            return RedirectToAction(actionName: "index", controllerName: "home");
        }
    }
}