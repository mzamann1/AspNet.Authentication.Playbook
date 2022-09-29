using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;

namespace Pluralsight.AspNetCore.Auth.Web.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        [Route("sign-in")]
        public IActionResult SignIn()
        {
            return View();
        }
        [Route("sign-in/{provider}")]
        public IActionResult SignIn(string provider, string returnUrl=null)
        {
            return Challenge(new AuthenticationProperties()
            {
                RedirectUri = returnUrl ?? "/"
            }, provider);
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
