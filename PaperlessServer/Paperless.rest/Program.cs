using EasyNetQ;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Paperless.rabbitmq;
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
        public static async Task Main(string[] args)
        {

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole()
                    .AddDebug();
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();

            var message = new Message { Text = "Hello" };
            /*
            using (var bus = RabbitHutch.CreateBus("host = localhost;username = guest;password = guest"))
            {
                bus.PubSub.Publish<Message>(message);
                Console.WriteLine("Listening for messages. Hit <return> to quit.");
            }
            */
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create the host builder.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .UseUrls("http://0.0.0.0:8000/");
                });
    }
}
