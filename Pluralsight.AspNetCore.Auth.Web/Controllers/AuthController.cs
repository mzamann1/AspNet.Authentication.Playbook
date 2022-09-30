using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Pluralsight.AspNetCore.Auth.Web.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    [Route("sign-in")]
    public IActionResult SignIn()
    {
        return Challenge(new AuthenticationProperties() { RedirectUri = "/home" });
    }

    [HttpPost,Route("sign-out")]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);

    }
}