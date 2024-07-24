namespace ProductCatalogService.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        Task<string> GetId(string token);
    }
}
