using Microsoft.AspNet.Mvc;
using System;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;

        public AppController(IMailService service)
        {
            _mailService = service;
        }

        public IActionResult Index()
        {
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
