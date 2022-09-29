using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;
using Pluralsight.AspNetCore.Auth.Web.Model;
using Pluralsight.AspNetCore.Auth.Web.Services;

namespace Pluralsight.AspNetCore.Auth.Web.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService _userService)
        {
            this._userService = _userService;
        }

        [Route("sign-in")]
        public async Task<IActionResult> SignIn()
        {
            var authResult = await HttpContext.AuthenticateAsync("Temporary");
            if (authResult.Succeeded)
            {
                return RedirectToAction("Profile");
            }
            return View();
        }
        [Route("sign-in/{provider}")]
        public IActionResult SignIn(string provider, string returnUrl = null)
        {
            var redirectUri = Url.Action("Profile");

            if (returnUrl != null)
            {
                redirectUri += "?returnUrl=" + returnUrl;
            }
            return Challenge(new AuthenticationProperties()
            {
                RedirectUri = redirectUri
            }, provider);
        }

        [Route("sign-out")]
        [HttpPost]

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync("Temporary");

            return RedirectToAction("Index", "Home");
        }

        [Route("access-denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("sign-in/profile")]
        public async Task<IActionResult> Profile(string returnUrl = null)
        {
            var authResult = await HttpContext.AuthenticateAsync("Temporary");
            if (authResult.Succeeded)
            {
                var user = await _userService.GetById(authResult.Principal.FindFirst(ClaimTypes.NameIdentifier).Value);

                if (user != null)
                {
                    return await SignInUser(user, returnUrl);
                }

                var model = new ProfileModel()
                {
                    DisplayName = authResult.Principal.Identity.Name,
                };

                var emailClaim = authResult.Principal.FindFirst((ClaimTypes.Email));

                if (emailClaim != null)
                {
                    model.Email = emailClaim.Value;
                }

                return View(model);

            }
            return RedirectToAction("SignIn");
        }

        [HttpPost]
        [Route("sign-in/profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileModel model, string returnUrl)
        {
            var authResult = await HttpContext.AuthenticateAsync("Temporary");

            if (!authResult.Succeeded)
            {
                return RedirectToAction("SignIn");
            }

            if (ModelState.IsValid)
            {
                var user = await _userService.Create(id: authResult.Principal.FindFirst(ClaimTypes.NameIdentifier).Value,
                     displayName: model.DisplayName, email: model.Email);

                return await SignInUser(user, returnUrl);
            }


            return View(model);

        }

        private async Task<IActionResult> SignInUser(User user, string returnUrl)
        {
            await HttpContext.SignOutAsync("Temporary");

            var claims = new List<Claim>()
           {
               new Claim(ClaimTypes.NameIdentifier,user.Id),
               new Claim(ClaimTypes.Name,user.DisplayName),
               new Claim(ClaimTypes.Email,user.Email)
           };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Redirect(returnUrl ?? "/home");
        }
    }
}














