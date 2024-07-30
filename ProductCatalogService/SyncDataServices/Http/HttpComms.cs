

namespace ProductCatalogService.SyncDataServices.Http
{
    public class HttpComms : IHttpComms
    {
        private readonly IConfiguration _configuration;

        public HttpComms(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetUserId(string token)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"{_configuration.GetConnectionString("AuthService")}/GetUserId?token={token}");
                
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"HTTP Communication Here: ProductCatalogService -> AuthService 🔥🔥🔥");

                    Console.WriteLine("Event: To retrieve ownerId of user from AuthService");

                    Console.WriteLine($"AuthService URL: {_configuration.GetConnectionString("AuthService")}");

                    return await response.Content.ReadAsStringAsync();
                }

                return string.Empty;
            }
        }

        public async Task<bool> IsTokenValid(string token)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"{_configuration.GetConnectionString("AuthService")}/IsTokenValid?token={token}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"HTTP Communication Here: ProductCatalogService -> AuthService 🔥🔥🔥");

                    Console.WriteLine("Event: To validate token of user from AuthService");

                    Console.WriteLine($"AuthService URL: {_configuration.GetConnectionString("AuthService")}");

                    string result = await response.Content.ReadAsStringAsync();
                   
                    if(result != null)
                    {
                        return bool.TryParse(result, out bool res);
                    }
                    
                    return false;
                }

                return false;
            }
        }

        public async Task<string> TestConnection()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"{_configuration.GetConnectionString("AuthService")}/TestConnection");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"HTTP Communication Here: ProductCatalogService -> AuthService 🔥🔥🔥");

                    Console.WriteLine("Event: To test connection from AuthService");

                    Console.WriteLine($"AuthService URL: {_configuration.GetConnectionString("AuthService")}");

                    string message = await response.Content.ReadAsStringAsync();

                    return message;
                }

                Console.WriteLine("Http test connection failed on AuthService");
                
                return string.Empty;
            }
        }
    }
}
