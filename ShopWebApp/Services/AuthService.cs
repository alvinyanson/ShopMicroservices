using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShopWebApp.Services
{
    public class AuthService
    {
        private const string TOKEN_NAME = "login";

        public string Scheme => CookieAuthenticationDefaults.AuthenticationScheme;

        public async Task SignInAsync(HttpContext context, string token)
        {
            context.Response.Cookies.Append(TOKEN_NAME, token);

            var userIdentity = new ClaimsIdentity(GetClaims(token), Scheme);

            var principal = new ClaimsPrincipal(userIdentity);

            await context.SignInAsync(Scheme, principal);
        }

        public async Task SignOutAsync(HttpContext context)
        {
            await context.SignOutAsync(Scheme);

            context.Response.Cookies.Delete(TOKEN_NAME);
        }

        public static IEnumerable<Claim> GetClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(token);

            return securityToken.Claims;
        }

        public async Task RefreshTokenAsync(HttpContext context)
        {
            string? token = GetToken(context);

            if (!string.IsNullOrEmpty(token))
            {
                await SignInAsync(context, token);
            }
        }

        public string? GetToken(HttpContext context)
        {
            return context.Request.Cookies[TOKEN_NAME];
        }
    }
}
