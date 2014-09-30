using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace IdentityApplicationAPI.Controllers
{
    [Authorize]
    public class IdentityController : ApiController
    {
        [Authorize(Roles = "student,teacher")]
        [Scope("write")]
        public dynamic Get()
        {

            System.Security.Claims.ClaimsPrincipal userClaims = new ClaimsPrincipal(User.Identity);

            if (userClaims.IsInRole("student"))
            {

            }

            var principal = User as ClaimsPrincipal;

            return from c in principal.Identities.First().Claims
                   select new
                   {
                       c.Type,
                       c.Value
                   };
        }

        [Authorize(Roles = "teacher")]        
        public dynamic Get(int id)
        {
            System.Security.Claims.ClaimsPrincipal userClaims = new ClaimsPrincipal(User.Identity);

            if (userClaims.IsInRole("teacher"))
            {

            }

            var principal = User as ClaimsPrincipal;

            return from c in principal.Identities.First().Claims
                   select new
                   {
                       c.Type,
                       c.Value
                   };
        }
    }
}
