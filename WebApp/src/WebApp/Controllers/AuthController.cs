using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<WebAppUser> _signInManager;

        public AuthController(SignInManager<WebAppUser> singInManager)
        {
            _signInManager = singInManager;
        }

        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Trips", "WebApp");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, false);

                if(signInResult.Succeeded)
                {
                    if(string.IsNullOrWhiteSpace(returnUrl))
                    {
                        RedirectToAction("Trips", "WebApp");
                    }
                    else
                    {
                        Redirect(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Login or password incorrect");
                }
            }

            return View();
        }
    }
}
