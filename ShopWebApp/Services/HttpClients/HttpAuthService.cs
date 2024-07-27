using ShopWebApp.Services.Contracts;

namespace ShopWebApp.Services.HttpClients
{
    public class HttpAuthService : IHttpService
    {
        public string Name => "AuthService";

        public string NormalizedName => "Auth Service";
    }
}
