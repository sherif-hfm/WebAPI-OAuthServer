using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace OAuthServer
{
    public class MyOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        MemoryCache memCache = MemoryCache.Default;
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // to Check basic auth to validate request 
            // validate client credentials
            // should be stored securely (salted, hashed, iterated)
            string id, secret;
            if (context.TryGetBasicCredentials(out id, out secret))
            {
                if (secret == "asd")
                {
                    context.Validated();
                }
            }
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // validate user credentials (demo mode)
            // should be stored securely (salted, hashed, iterated)        
            if (context.UserName != context.Password)
            {
                context.Rejected();
                return;
            }

            // create identity
            var id = new ClaimsIdentity(context.Options.AuthenticationType);
            id.AddClaim(new Claim(ClaimTypes.Name, "Sherif"));
            id.AddClaim(new Claim("sub", context.UserName));
            id.AddClaim(new Claim("role", "user"));
            var userGuid = Guid.NewGuid().ToString();
            id.AddClaim(new Claim("UserGuid", userGuid));
            memCache.Add(userGuid, "OK", DateTimeOffset.UtcNow.AddMinutes(5));
            // create metadata to pass on to refresh token provider
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { "as:client_id", context.ClientId }
                });

            var ticket = new AuthenticationTicket(id, props);
            context.Validated(ticket);
        }

        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            var userGuid = context.Ticket.Identity.Claims.Where(c => c.Type == "UserGuid").First().Value;
            if (!memCache.Contains(userGuid, null))
            {
                context.Rejected();
                return;
            }
            

            // enforce client binding of refresh token
            if (originalClient != currentClient)
            {
                context.Rejected();
                return;
            }

            // chance to change authentication ticket for refresh token requests
            //var newId = new ClaimsIdentity(context.Ticket.Identity);
            ////newId.AddClaim(new Claim("tokenId", Guid.NewGuid().ToString()));

            //var newTicket = new AuthenticationTicket(newId, context.Ticket.Properties);
            //context.Validated(newTicket);
            context.Validated(context.Ticket);
        }
    }
}
