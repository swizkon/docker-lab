using System;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace SayHelloApp.Controllers
{
    public class MailController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public MailController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        // GET: Mail
        public ActionResult Index()
        {
            _distributedCache.Set("MailController", Encoding.UTF8.GetBytes(DateTime.Now.ToString(CultureInfo.InvariantCulture)));

            return View();
        }

        // POST: Mail/Create
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                using (var message = new MailMessage(from: "sender@example.com", to: "someone@else.com", subject: "Subject", body: "The body")
                {
                    IsBodyHtml = false
                })
                {
                    using (var mailserver = new SmtpClient("mailserver", 1025))
                    {

                        mailserver.Send(message);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}