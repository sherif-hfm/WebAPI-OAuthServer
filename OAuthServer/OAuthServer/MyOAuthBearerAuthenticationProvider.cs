using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;

namespace OAuthServer
{
    public class MyOAuthBearerAuthenticationProvider : IOAuthBearerAuthenticationProvider
    {
        MemoryCache memCache = MemoryCache.Default;
        public Task ApplyChallenge(OAuthChallengeContext context)
        {
            //if token invalid

            //context.Response.Redirect("/Account/Login");
            return Task.FromResult<object>(null);
        }

        public Task RequestToken(OAuthRequestTokenContext context)
        {
            // apply token
            return Task.FromResult<object>(null);
        }

        public Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
            // if token valid

            var userGuid = context.Ticket.Identity.Claims.Where(c => c.Type == "UserGuid").First().Value;
            if (!memCache.Contains(userGuid, null))
            {
                context.Rejected(); // if i want to reject the token
                return null;
            }

            return Task.FromResult<object>(null);
        }
    }
}