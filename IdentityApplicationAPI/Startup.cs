using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Owin;
using Owin;
using System.IdentityModel.Tokens;
using System.Linq;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.Owin.ScopeValidation;
using Thinktecture.IdentityModel.Tokens;
using Thinktecture.IdentityServer.v3.AccessTokenValidation;

[assembly: OwinStartup(typeof(IdentityApplicationAPI.Startup))]
namespace IdentityApplicationAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = Thinktecture.IdentityModel.Tokens.ClaimMappings.None;

            // for self contained tokens
            app.UseIdentitiyServerJwt(new JwtTokenValidationOptions
            {
                Authority = "http://dispatch.ru.is/auth/core", 
                //AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Passive
            });

            // for reference tokens
            app.UseIdentitiyServerReferenceToken(new ReferenceTokenValidationOptions
            {
                Authority = "http://dispatch.ru.is/auth/core", 
                //AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Passive
            });

            // require read OR write scope
            app.RequireScopes(new ScopeValidationOptions
            {
                AllowAnonymousAccess = true,
                Scopes = new[] { "read", "write" }
            });

            app.UseWebApi(WebApiConfig.Register());
        }
    }
}