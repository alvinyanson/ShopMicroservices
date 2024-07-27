using ShopWebApp.Services.Contracts;

namespace ShopWebApp.Services.HttpClients
{
    public class HttpProductCatalogService : IHttpService
    {
        public string Name => "ProductCatalogService";

        public string NormalizedName => "Product Catalog Service";
    }
}
