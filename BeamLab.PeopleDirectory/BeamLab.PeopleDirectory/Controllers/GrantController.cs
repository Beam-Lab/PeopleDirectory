﻿using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace BeamLab.PeopleDirectory.Controllers
{
    public class GrantController : ApiController
    {
        private const string tenantIdClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";

        private const string authorityFormat = "https://login.microsoftonline.com/{0}/v2.0";
        private const string msGraphScope = "https://graph.microsoft.com/.default";
        private const string msGraphQuery = "https://graph.microsoft.com/v1.0/users/";

        // GET api/<controller>
        public async Task<IEnumerable<string>> Get()
        {
            var tenantId = ClaimsPrincipal.Current.FindFirst(tenantIdClaimType).Value;

            ConfidentialClientApplication daemonClient = new ConfidentialClientApplication(Startup.clientId, String.Format(authorityFormat, tenantId), Startup.redirectUri, new ClientCredential(Startup.clientSecret), null, new TokenCache());
            AuthenticationResult authResult = await daemonClient.AcquireTokenForClientAsync(new string[] { msGraphScope });

            // Query for list of users in the tenant
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, msGraphQuery);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
            HttpResponseMessage response = await client.SendAsync(request);

            // If the token we used was insufficient to make the query, drop the token from the cache.
            // The Users page of the website will show a message to the user instructing them to grant
            // permissions to the app (see User/Index.cshtml).
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                // BUG: Here, we should clear MSAL's app token cache to ensure that on a subsequent call
                // to SyncController, MSAL does not return the same access token that resulted in this 403.
                // By clearing the cache, MSAL will be forced to retrieve a new access token from AAD, 
                // which will contain the most up-to-date set of permissions granted to the app. Since MSAL
                // currently does not provide a way to clear the app token cache, we have commented this line
                // out. Thankfully, since this app uses the default in-memory app token cache, the app still
                // works correctly, since the in-memory cache is not persistent across calls to SyncController
                // anyway. If you build a persistent app token cache for MSAL, you should make sure to clear 
                // it at this point in the code.
                //
                //daemonClient.AppTokenCache.Clear(Startup.clientId);
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpResponseException(response.StatusCode);
            }

            return null;
        }
    }
}