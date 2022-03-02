using ECommerce.Models.Entities;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace ECommerce.IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticateController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticateController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IIdentityServerInteractionService interaction,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _environment = environment;
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
            public string ReturnUrl { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var context = await _interaction.GetAuthorizationContextAsync(request.ReturnUrl);

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, request.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(request.Username);
                    if (user != null && context != null)
                    {
                        return new JsonResult(new { RedirectUrl = request.ReturnUrl, IsOk = true });
                    }
                }
            }

            return Unauthorized();
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            bool showSignoutPrompt = true;

            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                showSignoutPrompt = false;
            }

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await HttpContext.SignOutAsync();
            }

            // no external signout supported for now (see \Quickstart\Account\AccountController.cs TriggerExternalSignout)
            return Ok(new
            {
                showSignoutPrompt,
                ClientName = string.IsNullOrEmpty(context?.ClientName) ? context?.ClientId : context?.ClientName,
                context?.PostLogoutRedirectUri,
                context?.SignOutIFrameUrl,
                logoutId
            });
        }

        [HttpGet]
        [Route("Error")]
        public async Task<IActionResult> Error(string errorId)
        {
            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);

            if (message != null)
            {
                if (!_environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return Ok(message);
        }
    }
}