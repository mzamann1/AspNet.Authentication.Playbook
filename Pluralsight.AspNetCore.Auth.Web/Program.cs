using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pluralsight.AspNetCore.Auth.Web.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMvc(options => options.EnableEndpointRouting = true);

builder.Services.AddAuthentication(option =>
    {
        option.DefaultSignInScheme = "Temporary";
        //option.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme; for external only challenge
        option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    }).AddCookie(option =>
    {
        option.LoginPath = "/auth/sign-in";
    })
    .AddCookie("Temporary")
    .AddFacebook(fbOptions =>
    {
        fbOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"]; //from secret manager
        fbOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]; //from secret manager
        fbOptions.AccessDeniedPath = "/auth/access-denied";
    });

builder.Services.AddSingleton<IUserService, DummyUserService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();