using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pluralsight.AspNetCore.Auth.Web.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {



            var serverClient = _clientFactory.CreateClient("IDSClient");
            var discovery = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44338");

            var response = await serverClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = discovery.TokenEndpoint,
                ClientId = "WebApi",
                ClientSecret = "MySecret",
                Scope = "DemoApiScope"

            });

            object model;
            if (response.IsError)
            {
                model = "Error... could not get access token from server";
            }
            else
            {
                var apiClient = _clientFactory.CreateClient("WebApiClient");
                apiClient.SetBearerToken(response.AccessToken);

                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"/api/text/welcome");

                var apiResponse = await apiClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                if (apiResponse.IsSuccessStatusCode)
                {
                    model = await apiResponse.Content.ReadAsStringAsync();
                }
                else
                {
                    model = "Error... couldnot get data from api";

                }

            }

            return View(model);
        }



        [Route("userinformation")]
        [Authorize]
        public IActionResult UserInformation()
        {
            return View();
        }
    }
}
