using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BeamLab.PeopleDirectory
{
    public partial class Startup
    {
        //PRODUZIONE
        public static string clientId = "7ef0321a-3580-4db9-819c-6e54ee169a3e";
        public static string clientSecret = "ZLPpu2GfsHmEnCM5vV2jTbo";

        //public static string clientId = "1b21a431-adaf-4d83-9f48-5ad89be03fa4";
        //public static string clientSecret = "psSoNLEArQ2XGbKsvjfiRCz";

        private static string authority = "https://login.microsoftonline.com/common/v2.0";
		public static string redirectUri = "https://localhost:44316/";

        //public static string redirectUri = "https://smartflow365webapi.azurewebsites.net/";

        private void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = clientId,
                Authority = authority,
                RedirectUri = redirectUri,
                PostLogoutRedirectUri = redirectUri,
                Scope = "openid profile",
                ResponseType = "id_token",
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    NameClaimType = "name"
                },
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = OnAuthenticationFailed,
                    SecurityTokenValidated = OnSecurityTokenValidated,
                }
            });
        }

        private Task OnSecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            // Make sure that the user didn't sign in with a personal Microsoft account
            if (notification.AuthenticationTicket.Identity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value == "9188040d-6c67-4c5b-b112-36a304b66dad")
            {
                notification.HandleResponse();
                notification.Response.Redirect("/Account/UserMismatch");
            }

            return Task.FromResult(0);
        }

        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            notification.HandleResponse();
            notification.Response.Redirect("/Home/Error");
            return Task.FromResult(0);
        }
    }
}