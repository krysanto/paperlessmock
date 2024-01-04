using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Paperless.rabbitmq;
using Paperless.services;
using System;
using System.Threading.Tasks;

namespace Paperless.rest
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>

        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .UseUrls("http://0.0.0.0:8000/")
                    .ConfigureServices((hostContext, services) =>
                     {
                         services.AddHostedService<HostedService>();
                         // ... other registrations
                     });
                });
        }
    }
}


/*

Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<HostedService>();
                    // ... other registrations
                })
                .Build()
                .Run();
*/