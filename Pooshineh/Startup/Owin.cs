using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Pooshineh.Startup
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure authentication using cookies
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Accounts/Login"), // Customize the login path
                                                               // Other options...
            });

            // Add other middleware here if needed
        }
    }
}
