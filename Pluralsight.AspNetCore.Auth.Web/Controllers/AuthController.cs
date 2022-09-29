using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;

namespace Pluralsight.AspNetCore.Auth.Web.Controllers
{
    [Microsoft.AspNetCore.Components.Route("auth")]
    public class AuthController : Controller
    {
        [Route("sign-in")]
        public IActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties()
            {
                RedirectUri = "/"
            });
        }

        [Route("sign-out")]
        [HttpPost]

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [Route("access-denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
