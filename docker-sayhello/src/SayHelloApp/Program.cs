using System;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace SayHelloApp
{
    public class Program
    {
        private const string UrlFlag = "--useurl=";

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var webHostBuilder
                = WebHost.CreateDefaultBuilder(args)
                         .ConfigureAppConfiguration(builder => builder.AddCommandLine(args))
                         .UseStartup<Startup>()
                         .ConfigureLogging(logging =>
                         {
                             logging.ClearProviders();
                             logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                         })
                         .UseNLog(NLogAspNetCoreOptions.Default);

            var useUrl
                = args.Where(x => x.StartsWith(UrlFlag, StringComparison.InvariantCultureIgnoreCase))
                      .Select(x => x.Replace(UrlFlag, "", StringComparison.InvariantCultureIgnoreCase))
                      .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(useUrl))
            {
                webHostBuilder = webHostBuilder.UseUrls(useUrl);
            }

            return webHostBuilder;
        }
    }
}