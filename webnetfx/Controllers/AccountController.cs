using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

namespace webnetfx.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "123456")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };

                var identity = new ClaimsIdentity(claims, "Cookies");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true,// cookie persiste
                }, identity);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Credenciales inv�lidas.");
            return View();
        }
    }
}