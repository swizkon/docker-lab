using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SayHelloApp.Models;

namespace SayHelloApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        private static readonly HttpClient httpClient = new HttpClient();

        public HomeController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILogger<HomeController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Index page says hello at {Timestamp}", DateTime.Now);
            
            var myServiceDetails = GetServiceDetails();
            return View(model: myServiceDetails);
        }

        public async Task<IActionResult> Siblings()
        {
            var siblings = _configuration.GetValue<string>("SayHello:Siblings");
            ViewBag.Siblings = siblings;
            
            var siblingDetailsTasks = GetSiblingDetails(siblings.Split(' '), "The sender");

            var siblingDetails = await Task.WhenAll(siblingDetailsTasks);

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
