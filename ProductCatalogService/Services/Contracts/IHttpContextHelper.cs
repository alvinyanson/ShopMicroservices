namespace ProductCatalogService.Services.Contracts
{
    public interface IHttpContextHelper
    {
        string GetTokenFromHeaders();
    }
}
