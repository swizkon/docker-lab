using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using SayHelloApp.Models;

namespace SayHelloApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly CloudBlobClient _cloudBlobClient;
        private readonly ILogger<HomeController> _logger;

        private static readonly HttpClient httpClient = new HttpClient();

        private readonly IDistributedCache _distributedCache;

        public HomeController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, CloudBlobClient cloudBlobClient, IDistributedCache distributedCache, ILogger<HomeController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _distributedCache = distributedCache;
            _cloudBlobClient = cloudBlobClient;
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var cacheKey = "MailController";
            _distributedCache.Set(cacheKey, Encoding.UTF8.GetBytes(DateTime.Now.ToString(CultureInfo.InvariantCulture)));

            _logger.LogInformation("Index page says hello at {Timestamp}", DateTime.Now);
            
            var container = _cloudBlobClient.GetContainerReference("mycontainer");
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlockBlobReference("myblob-" + DateTime.Now.Minute);
            
            await blob.UploadTextAsync("Test it at " + DateTime.Now.ToString());
            

            var d = _distributedCache.Get(cacheKey);
            
            var myServiceDetails = GetServiceDetails();
            return View(model: myServiceDetails);
        }

        [Authorize]
        public async Task<IActionResult> Siblings()
        {
            var siblings = _configuration.GetValue<string>("SayHello:Siblings");
            ViewBag.Siblings = siblings;
            
            var siblingDetailsTasks = GetSiblingDetails(siblings.Split(' '), "The sender");

            var siblingDetails = await Task.WhenAll(siblingDetailsTasks);

            var container = _cloudBlobClient.GetContainerReference("mycontainer");
            var blob = container.GetBlockBlobReference("myblob");
            
            var txt = await blob.DownloadTextAsync();

            BlobContinuationToken token = null;
            var data = container.ListBlobsSegmentedAsync(token);

            var docs = data.Result.Results.OfType<CloudBlockBlob>().Select(x => x.Name);

            siblingDetails = siblingDetails.Append(txt).Concat(docs).ToArray();
            
            return View(siblingDetails);
        }

        public IActionResult ServiceDetails()
        {
            var myServiceDetails = GetServiceDetails();
            return Json(myServiceDetails);
        }

        private ServiceDetailsModel GetServiceDetails()
        {
            return new ServiceDetailsModel
            {
                ServiceName = _configuration.GetValue<string>("SayHello:ServiceName"),
                Host = _httpContextAccessor.HttpContext.Request.Host.Host,
                Port = _httpContextAccessor.HttpContext.Request.Host.Port?.ToString(),
                IncomingHeaders = _httpContextAccessor.HttpContext?.Request?
                    .Headers.Select(k => new KeyValuePair<string, string>(k.Key, string.Join(", ", k.Value.ToArray()))).ToList()
            };
        }

        private Task<string>[] GetSiblingDetails(string[] services, string sender)
        {
            return services.Select(service => Task.Run(async () => await GetSiblingServiceDetails(service, sender))).ToArray();
        }

        private async Task<string> GetSiblingServiceDetails(string service, string sender)
        {
            try
            {
                var data = await httpClient.GetStringAsync(service + "/home/ServiceDetails");
                return data;
                //var model = JsonConvert.DeserializeObject<ServiceDetailsModel>(data);
                //return "Name: " + model.ServiceName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
