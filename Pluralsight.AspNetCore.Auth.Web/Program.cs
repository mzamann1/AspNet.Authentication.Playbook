using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddOpenIdConnect(options =>
    {
        options.Authority = "https://localhost:44338";
        options.ClientId = "AuthWeb";
        options.SaveTokens = true;
        options.TokenValidationParameters.NameClaimType = "name";
    })
    .AddCookie();

builder.Services.AddHttpClient("WebApiClient", options =>
{
    options.BaseAddress = new Uri("https://localhost:44313");
    options.DefaultRequestHeaders.Clear();
    options.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});


builder.Services.AddHttpClient("IDSClient", options =>
{
    options.BaseAddress = new Uri("https://localhost:44338");
    options.DefaultRequestHeaders.Clear();
});

var app = builder.Build();

LoggerFactory.Create(config =>
{
    config.AddConsole();
});


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