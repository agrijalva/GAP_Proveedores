using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Eprocurement.Compras.App_Start
{
    public static class LoginAuthentication
    {
        public const String ApplicationCookie = "GapAuthenticationType";
    }
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = LoginAuthentication.ApplicationCookie,
                LoginPath = new PathString("/SeguridadAD/Index"),
                Provider = new CookieAuthenticationProvider(),
                CookieName = "LoginCookie",
                CookieHttpOnly = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(300),
            });
        }
    }
}