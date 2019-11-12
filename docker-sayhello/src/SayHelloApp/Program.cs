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
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var useUrl = args.Where(x => x.StartsWith("--useurl="))
                             .Select(x => x.Replace("--useurl=", ""))
                             .FirstOrDefault();
 
            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                                        .ConfigureAppConfiguration(builder => builder.AddCommandLine(args))
                                        .UseStartup<Startup>()
                                        .ConfigureLogging(logging =>
                                        {
                                            logging.ClearProviders();
                                            logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                                        })
                                        .UseNLog();

            if (!string.IsNullOrWhiteSpace(useUrl))
            {
                webHostBuilder = webHostBuilder.UseUrls(useUrl);
            }

            return webHostBuilder;
        }
    }
}
