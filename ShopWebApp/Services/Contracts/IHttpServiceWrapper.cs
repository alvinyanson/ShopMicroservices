namespace ShopWebApp.Services.Contracts
{
    public interface IHttpServiceWrapper
    {
        IHttpService Service { get; }

        Task<HttpResponseMessage?> PostAsync(HttpContext context, object content, params string[] endpoints);

        Task<HttpResponseMessage?> GetAsync(HttpContext context, params string[] endpoints);

        Task<HttpResponseMessage?> DeleteAsync(HttpContext context, params string[] endpoints);

        IHttpServiceWrapper AddParameter(string name, object value);

        Task<bool> IsRunning();
    }
}
