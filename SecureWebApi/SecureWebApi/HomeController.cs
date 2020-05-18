using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SecureWebApi
{
    [RoutePrefix("home")]
    public class HomeController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        [Route("GetData")]
        [Authorize]
        public async Task<string> Get()
        {
            return await Task.FromResult("OK");
        }

       
    }
}