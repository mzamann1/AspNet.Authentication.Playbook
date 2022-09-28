using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        public async Task<IActionResult> SignIn([FromBody] SignInModel model, [FromServices] DummyUserService userService)
        {
            if (ModelState.IsValid)
            {
                User user;
                if (await userService.ValidateCredentials(model.Username, model.Password, out user))
                {
                    await SignInUser(model.Username);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
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
    }
}
















