using Microsoft.AspNet.Mvc;
using System;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;
using System.Linq;

namespace WebApp.Controllers.Web
{
    public class WebAppController : Controller
    {
        private IMailService _mailService;
        private WebAppContext _context;

        public WebAppController(IMailService service, WebAppContext context)
        {
            _context = context;
            _mailService = service;
        }

        public IActionResult Index()
        {
            var trips = _context.Trips.OrderBy(c => c.Name).ToList();
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if(ModelState.IsValid)
            {
                var email = Startup.Configuration["AppSettings:siteEmailAddress"];
                _mailService.SendMail(email, email, $"Contact from {model.Name} ({model.Email})", model.Message);

            }
            return View();
        }
    }
}
