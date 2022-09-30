using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PluralSight.AspNetCore.Auth.Api.Controllers;

[Produces("application/json"), Route("api")]
public class ApiController : Controller
{
    [Route("text/welcome"), Authorize]
    public IActionResult GetWelcomeText() => Content("Welcome " + User.Identity.Name);
}