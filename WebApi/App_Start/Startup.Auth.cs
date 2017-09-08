using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace WebApi
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Add bearer token to Authorization header from ReturnUrlParameter 
            app.Use<HeadersResquetAuthorizationBearerMiddleware>();

            // The OWIN cookie middleware will redirect unauthorized requests to the login page
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = "JWT",
                AuthenticationMode = AuthenticationMode.Active,
                CookieName = "JWTFormat",
                LoginPath = PathString.FromUriComponent("/Auth/Login"),
                Provider = new CookieAuthenticationProvider()
                {
                    OnResponseSignIn = ResponseSignIn,
                    OnResponseSignOut = ResponseSignout,
                    OnApplyRedirect = ApplyRedirectToLoginPage
                },
                CookieManager = new SystemWebCookieManager()
            });
            // Enable JWT Bearer Authentification 
            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions()
            {
                AuthenticationType = "JWT",
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] { System.Configuration.ConfigurationManager.AppSettings["TokenAudience"] },
                IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[] {
                    new SymmetricKeyIssuerSecurityTokenProvider(System.Configuration.ConfigurationManager.AppSettings["TokenIssuer"], Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["TokenKey"].ToString()))
                }
            });
        }



        private static void ApplyRedirectToLoginPage(CookieApplyRedirectContext context)
        {
            Uri absolueURI;
            if (Uri.TryCreate(context.RedirectUri, UriKind.Absolute, out absolueURI))
            {
                var path = PathString.FromUriComponent(absolueURI);
                if (path == context.OwinContext.Request.PathBase + context.Options.LoginPath)
                {
                    context.RedirectUri = "http://localhost:55821/Account/Login" + new QueryString(context.Options.ReturnUrlParameter, context.Request.Uri.AbsoluteUri);
                }
            }

            context.Response.Redirect(context.RedirectUri);

        }
        private void ResponseSignIn(CookieResponseSignInContext context)
        {
            context.Options.CookieDomain = context.Request.Uri.Host;
        }

        private void ResponseSignout(CookieResponseSignOutContext context)
        {
            context.Options.CookieDomain = context.Request.Uri.Host;
        }
    }
}