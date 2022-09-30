using Microsoft.AspNetCore.Mvc;

namespace Pluralsight.AspNetCore.Auth.Web.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    [Route("sign-out"), HttpPost]
    public IActionResult Logout()
    {
        throw new System.NotImplementedException();
    }

    [Route("sign-in")]
    public IActionResult SignIn()
    {
        throw new System.NotImplementedException();
    }
}