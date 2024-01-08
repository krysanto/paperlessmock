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
            Thread.Sleep(17500);
            CreateHostBuilder(args).Build().Run();      
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .UseUrls("http://0.0.0.0:8001/")
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<HostedService>();
                        // ... other registrations
                    });
                });
    }
}