using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using System.Web.Security;

[assembly: OwinStartup(typeof(SecureWebApi.Startup))]

namespace SecureWebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Do not forget to make machineKey in web.config the same in server and client

            /* Postman request
             GET /home/GetData HTTP/1.1
            Host: localhost:3212
            Authorization: Bearer gSEUE_L7wc1vbZlulCQi9x_MVkbGufmSAdvye6T1kvopNfcNq29WPdQ98DqGLdtw1eXkkwgkF3V5bJwyTaGXfz4hAANsUl0e3pMWl37RQ5wAC51RXHbA6TlMKrilS4ZiegEBL5erRE61heyyIAaP9KBYXbTgt_cG8SY6vLhHeVBzrJLNwsGJGRC18sj9Qa3Sr4TZieL2eT-vyxVCLeq1u_F0N9H2hrEvxysKX8r1PsEup1-8
            Cache-Control: no-cache
            Postman-Token: ec959813-2035-4550-a5dd-715703819a5f
             */


            // token consumption
            app.SetDataProtectionProvider(new MachineKeyProtectionProvider()); //// token generation Need when use sels host
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
            { AuthenticationMode = AuthenticationMode.Active });

            // Web API routes
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
    internal class MachineKeyProtectionProvider : IDataProtectionProvider
    {
        public IDataProtector Create(params string[] purposes)
        {
            return new MachineKeyDataProtector(purposes);
        }
    }

    internal class MachineKeyDataProtector : IDataProtector
    {
        private readonly string[] _purposes;

        public MachineKeyDataProtector(string[] purposes)
        {
            _purposes = purposes;
        }

        public byte[] Protect(byte[] userData)
        {
            return MachineKey.Protect(userData, _purposes);
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, _purposes);
        }
    }


   
}
