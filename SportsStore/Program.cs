using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = BuildWebHost(args);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseDefaultServiceProvider(options=>options.ValidateScopes=false)
                    .Build();
                });

        public static IHost BuildWebHost(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(Web => {
            Web.UseStartup<Startup>();
        })
        .Build();
    }
}
