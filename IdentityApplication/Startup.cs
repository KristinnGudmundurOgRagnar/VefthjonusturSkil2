using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Security.Claims;
//using Thinktecture.IdentityModel.Tokens;


namespace IdentityApplication
{
    public static class Constants
    {
        public const string BaseAddress = "http://dispatch.ru.is/auth/core";
        //public const string BaseAddress = "http://localhost:3333/core";
        //public const string BaseAddress = "http://idsrv3.azurewebsites.net/core";

        /*public const string AuthorizeEndpoint = BaseAddress + "/connect/authorize";
        public const string LogoutEndpoint = BaseAddress + "/connect/endsession";
        public const string TokenEndpoint = BaseAddress + "/connect/token";
        public const string UserInfoEndpoint = BaseAddress + "/connect/userinfo";

        public const string AspNetWebApiSampleApi = "http://localhost:2727/";*/
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = "implicitclient",
                Authority = Constants.BaseAddress,
                RedirectUri = "http://localhost:62838/", // The URL of the application calling
                ResponseType = "id_token token",
                Scope = "openid profile email read write",

                SignInAsAuthenticationType = "Cookies",


                // sample how to access token on form (for token response type)
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    MessageReceived = async n =>
                    {
                        var token = n.ProtocolMessage.Token;

                        if (!string.IsNullOrEmpty(token))
                        {
                            n.OwinContext.Set<string>("idsrv:token", token);
                        }
                    },
                    SecurityTokenValidated = async n =>
                    {
                        var token = n.OwinContext.Get<string>("idsrv:token");

                        if (!string.IsNullOrEmpty(token))
                        {
                            n.AuthenticationTicket.Identity.AddClaim(
                                new Claim("access_token", token));
                        }
                    }
                }
            });
        }
    }
}