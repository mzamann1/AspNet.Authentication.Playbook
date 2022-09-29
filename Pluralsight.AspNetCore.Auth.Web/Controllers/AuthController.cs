using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Pluralsight.AspNetCore.Auth.Web.Models;
using Pluralsight.AspNetCore.Auth.Web.Services;

namespace Pluralsight.AspNetCore.Auth.Web.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        [Route("sign-in")]
        public IActionResult SignIn()
        {
            return View(new SignInModel());
        }



        [Route("sign-in")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model, [FromServices] IUserService userService, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                User user;
                if (await userService.ValidateCredentials(model.Username, model.Password, out user))
                {
                    await SignInUser(model.Username);

                    if (returnUrl != null)
                    {
                        if (Url.IsLocalUrl(returnUrl))
                            return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("Login Error", "Invalid Credentials");
            }
            return View(model);
        }

        [HttpPost]
        [Route("sign-out")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("index", "Home");

        }

        public async Task SignInUser(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name,username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);



        }


        [HttpGet]
        [Route("register")]
        public IActionResult SignUp(string returnUrl = null)
        {
            return View(new SignUpModel() { returnUrl = returnUrl });
        }


        [HttpPost]
        [Route("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpModel model, [FromServices] IUserService userService)
        {
            if (ModelState.IsValid)
            {
                if (await userService.AddUser(model.Username, model.Password))
                {
                    await SignInUser(model.Username);

                    if (model.returnUrl != null)
                    {
                        if (Url.IsLocalUrl(model.returnUrl))
                            return Redirect(model.returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("Error", "User Already Exists");
            }
            return View(model);
        }
    }
}
















