using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace OAuthServer
{
    [RoutePrefix("home")]
    public class HomeController : ApiController
    {
        MemoryCache memCache = MemoryCache.Default;
        // GET api/<controller>
        [HttpGet]
        [Route("GetData")]
        [Authorize()]
        public async Task<string> Get()
        {
            var user=(ClaimsPrincipal)User;
            return await Task.FromResult("OK User:" + user.Identity.Name);
        }

        [HttpGet]
        [Route("Logout")]
        [Authorize()]
        public async Task<object> Logout()
        {
            var user = (ClaimsPrincipal)User;
            var userGuid = user.Claims.Where(c => c.Type == "UserGuid").First().Value;
            if (memCache.Contains(userGuid, null))
                memCache.Remove(userGuid);
            return await Task.FromResult<object>(null);
        }

    }
}