using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SayHelloApp.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }

        // GET: Mail/Create
        public ActionResult Create()
        {
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
                return View();
            }
        }
    }
}