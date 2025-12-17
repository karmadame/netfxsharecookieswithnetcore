using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Interop;

using Owin;
using System;
using System.IO;
using Microsoft.AspNetCore.DataProtection;

[assembly: OwinStartup(typeof(webnetfx.App_Start.Startup))]

namespace webnetfx.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            bool sharedCookie = true;
            IDataProtectionProvider dataProtectorProvider;
            DataProtectorShim shimDataProtector = null;
            
            if (sharedCookie)
            {
                dataProtectorProvider = DataProtectionProvider.Create(new DirectoryInfo(@"C:\keys"), config =>
                {
                    config.SetApplicationName("shared-app");
                });
                
                shimDataProtector = new DataProtectorShim(dataProtectorProvider.CreateProtector(
                    "Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware",
                    "Cookies",
                    "v2"
                ));
            }
            
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
                CookieName = ".AspNet.SharedCookie",
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(10),
                SlidingExpiration =  true,
                CookieHttpOnly = true,
                //CookieSameSite = SameSiteMode.Strict,  // para producción
                CookieSameSite = SameSiteMode.Lax, // para local
                CookieSecure = CookieSecureOption.SameAsRequest,
                TicketDataFormat = sharedCookie? new AspNetTicketDataFormat(shimDataProtector): null
            });
        }
    }
}
