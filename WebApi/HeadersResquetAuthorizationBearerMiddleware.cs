using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace WebApi
{
    public class HeadersResquetAuthorizationBearerMiddleware : OwinMiddleware
    {
        public HeadersResquetAuthorizationBearerMiddleware(OwinMiddleware next) : base(next)
        {
        }
        public override Task Invoke(IOwinContext context)
        {
            if (context.Request.QueryString.Value.Contains("Bearer"))
            {
                var jwtEncodedString = context.Request.Query.Get("Bearer");
                context.Request.Headers.Add("Authorization", new string[] { "Bearer " + jwtEncodedString });

                if (context.Authentication.User == null || !context.Authentication.User.Identity.IsAuthenticated)
                {
                    var jwtSecuritytoken = new JwtSecurityToken(jwtEncodedString);

                    var subjectValue = jwtSecuritytoken.Subject;

                    context.Authentication.SignIn(new ClaimsIdentity(new[] { new Claim(JwtRegisteredClaimNames.Sub, subjectValue) }, "JWT"));
                }
            }
            return Next.Invoke(context);
        }
    }
}