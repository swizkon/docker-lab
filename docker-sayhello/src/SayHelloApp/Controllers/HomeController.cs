using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SayHelloApp.Models;

namespace SayHelloApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILogger<HomeController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Index page says hello at {Timestamp}", DateTime.Now);

            //// Get minimal challenge data
            //var challenges = await _restApiClient.CallAsync(new Challenges.GetChallengesRequest(new Challenges.ChallengesResourceIdentifier()));

            //// Parallel get of details for each challenge
            //var challengeDetailsTasks = challenges.Select(c => Task.Run(async () => await GetChallengeDetails(c.Id))).ToArray();

            //// Wait for all to complete
            //var challengeDetails = await Task.WhenAll(challengeDetailsTasks);
            //// Format an output to the view
            //ViewBag.ChallengeData = string.Join(" ", challengeDetails.Select(x => x.Title + " " + x.Description + " from " + x.StartDate + " to " + x.EndDate));

            //// Get all teams
            //var teams = await _restApiClient.CallAsync(new Teams.GetTeamsRequest());
            //// Format an output to the view
            //ViewBag.TeamData = string.Join(" ", teams.Select(x => x.Name + " " + x.Department));
            
            // return View();
            var myServiceDetails = GetServiceDetails();
            return View(model: myServiceDetails);
        }

        public IActionResult Siblings()
        {
            var myServiceDetails = GetServiceDetails();
            return View(myServiceDetails);
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

        private async Task<string> GetAnotherServiceDetails(string service)
        {
            return await Task.FromResult("Hi! My name is {ServiceName} and Im running at end point {host}:{port}");
        }
    }
}
