using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TibiaPixelBot.Extensions
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;

            // Look for the LastChanged claim.
            var lastChanged = Convert.ToDateTime((from c in userPrincipal.Claims
                                                  where c.Type == ClaimTypes.Expiration
                                                  select c.Value).FirstOrDefault());

            if (lastChanged < DateTime.Now)
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(
                     CookieAuthenticationDefaults.AuthenticationScheme);
            }
            else
            {
                var claims = new List<Claim>();

                foreach (var loop in context.Principal.Claims)
                {
                    if (loop.Type == ClaimTypes.Expiration)
                    {
                        claims.Add(new Claim(loop.Type, DateTime.Now.AddMinutes(10).ToString(), loop.ValueType, loop.Issuer));
                    }
                    else
                    {
                        claims.Add(new Claim(loop.Type, loop.Value, loop.ValueType, loop.Issuer));
                    }
                }

                context.HttpContext.User = context.Principal;

                await context.HttpContext.SignInAsync(
                   scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                   principal: new ClaimsPrincipal(new ClaimsIdentity(claims, "cookie")));

                // context.ReplacePrincipal(new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
            }
        }

    }
}
