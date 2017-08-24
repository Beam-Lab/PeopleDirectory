using Microsoft.Identity.Client;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BeamLab.PeopleDirectory.Controllers
{
    public class InstallController : Controller
    {
        private const string tenantIdClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";
        private const string authorityFormat = "https://login.microsoftonline.com/{0}/v2.0";
        private const string msGraphScope = "https://graph.microsoft.com/.default";
        private const string msGraphQuery = "https://graph.microsoft.com/v1.0/users";

        public ActionResult Index()
        {
            // Make sure the user is signed in
            if (!Request.IsAuthenticated)
            {
                return new RedirectResult("/Account/SignIn");
            }
            //else
            //{
            //    return new RedirectResult("/Account/SignIn");
            //}

            return View();
        }

        [Authorize]
        public async Task<ActionResult> InstallationOk()
        {
            var tenantId = ClaimsPrincipal.Current.FindFirst(tenantIdClaimType).Value;

            var daemonClient = new ConfidentialClientApplication(Startup.clientId, String.Format(authorityFormat, tenantId), Startup.redirectUri, new ClientCredential(Startup.clientSecret), null, new TokenCache());
            var authResult = await daemonClient.AcquireTokenForClientAsync(new string[] { msGraphScope });

            return View();
        }

        [Authorize]
        public async Task<ActionResult> Grant()
        {
            var tenantId = ClaimsPrincipal.Current.FindFirst(tenantIdClaimType).Value;

            var daemonClient = new ConfidentialClientApplication(Startup.clientId, String.Format(authorityFormat, tenantId), Startup.redirectUri, new ClientCredential(Startup.clientSecret), null, new TokenCache());
            var authResult = await daemonClient.AcquireTokenForClientAsync(new string[] { msGraphScope });

            return View();
        }
    }
}