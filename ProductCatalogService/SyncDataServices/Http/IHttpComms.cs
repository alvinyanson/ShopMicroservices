namespace ProductCatalogService.SyncDataServices.Http
{
    public interface IHttpComms
    {
        // Available endpoints from AuthService
        Task<string> GetUserId(string token);

        Task<bool> IsTokenValid(string token);

        Task<string> TestConnection();
    }
}
