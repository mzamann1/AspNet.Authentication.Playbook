using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMvc(options => options.EnableEndpointRouting = true);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddOpenIdConnect(options =>
    {
        options.Authority = "https://login.microsoftonline.com/net6playbook.onmicrosoft.com";
        options.ClientId = "6c3a80f1-7669-44df-84cb-0ddd8b6d5e7f";
        options.ResponseType = OpenIdConnectResponseType.IdToken;
        options.CallbackPath = "/oauth/signin-callback";
        options.SignedOutRedirectUri = "https://localhost:44343/home";
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            NameClaimType = "name"
        };
    }).AddCookie();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();