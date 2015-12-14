using Microsoft.AspNet.Mvc;
using System;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;
using System.Linq;
using Microsoft.AspNet.Authorization;

namespace WebApp.Controllers.Web
{
    public class WebAppController : Controller
    {
        private IMailService _mailService;
        private IWebAppRepository _repository;

        public WebAppController(IMailService service, IWebAppRepository repository)
        {
            _repository = repository;
            _mailService = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Trips()
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
