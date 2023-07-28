using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace WebApplication2.Middleware
{
    public class CookieMiddleware
    {
        private readonly RequestDelegate _next;
        static string usuario = "";
        public CookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            {
                // Obter o cookie do request
                if (!context.Request.Cookies.TryGetValue(".ASPXAUTH", out string cookieValue))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                
                if (string.IsNullOrEmpty(cookieValue))
                {
                    
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
               if(context.Request.Headers["Usuario"].ToString() !="")
                {
                    usuario = context.Request.Headers["Usuario"].ToString();

                };
                // Se chegou até aqui, o cookie é válido
               
                var claims = new[]
                {
            new Claim(ClaimTypes.Name, usuario)
        };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);
                context.User = principal;             
                await _next(context);
            };
        }
    }
}
