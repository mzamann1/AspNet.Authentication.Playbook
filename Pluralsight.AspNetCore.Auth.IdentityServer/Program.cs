using IdentityServerHost.Quickstart.UI;
using Pluralsight.AspNetCore.Auth.IdentityServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(options => options.EnableEndpointRouting = true);

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
    .AddInMemoryClients(IdentityServerConfig.GetClients())
    .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
    .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
    .AddTestUsers(IdentityServerConfig.GetUsers());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

AccountOptions.ShowLogoutPrompt = false;
AccountOptions.AutomaticRedirectAfterSignOut = true;

app.UseIdentityServer();


app.MapDefaultControllerRoute();

app.Run();
