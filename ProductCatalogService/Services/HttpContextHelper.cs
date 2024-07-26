using ProductCatalogService.Services.Contracts;

namespace ProductCatalogService.Services
{
    public class HttpContextHelper : IHttpContextHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetTokenFromHeaders()
        {
            var request = _contextAccessor.HttpContext?.Request;

            if (request is null || !request.Headers.TryGetValue("Authorization", out var token))
            {
                return string.Empty;
            }

            return token.ToString();
        }
    }
}
