using BeamLab.PeopleDirectory.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BeamLab.PeopleDirectory.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public async Task<ActionResult> Index()
        {
            var list = List("3c8d03a5-57f9-4464-91d0-96130a1ef1d8", "");

            return View();
        }

        private async Task<TokenTenant> GetGraphToken(string tenantId)
        {
            var token = new TokenTenant();

            try
            {
                var daemonClient = new ConfidentialClientApplication(Startup.clientId,
                    String.Format(Constant.authorityFormat, tenantId), Startup.redirectUri,
                    new ClientCredential(Startup.clientSecret), null, new TokenCache());
                var authResult = await daemonClient.AcquireTokenForClientAsync(new string[] { Constant.msGraphScope });

                token.Token = authResult.AccessToken;
            }
            catch (Exception ex)
            {

            }

            return token;
        }

        public async Task<string> List(string tenantId, string email)
        {
            var usersQuery = $"{Constant.graphBasePathBeta}/users";

            var token = await GetGraphToken(tenantId);

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, usersQuery);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
            var response = await client.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();

            return json;
        }
    }
}