using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.Authority = "https://localhost:44338";
    options.Audience = "DemoApi";
    options.TokenValidationParameters.NameClaimType = "client_id";
});

builder.Services.AddAuthorization(option =>
{
    option.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
});

var app = builder.Build();


app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
