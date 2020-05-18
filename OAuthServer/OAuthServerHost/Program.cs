using Microsoft.Owin.Hosting;
using OAuthServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthServerHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://*:3010";
            using (var srv = WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
}
