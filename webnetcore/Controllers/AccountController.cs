using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace webnetcore.Controllers;

public class AccountController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public ActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> Login(string username, string password)
    {
        if (username == "admin" && password == "123456")
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, username));
            var principal = new ClaimsPrincipal(identity);

            await (_httpContextAccessor.HttpContext ?? throw new InvalidOperationException()).SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties
                {
                    IsPersistent = true
                });
            
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Credenciales inv�lidas.");
        return View();
    }
}