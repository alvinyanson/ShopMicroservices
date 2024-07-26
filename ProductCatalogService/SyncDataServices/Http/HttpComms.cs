

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
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public async Task<bool> IsTokenValid(string token)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"{_configuration.GetConnectionString("AuthService")}/isTokenValid?token={token}");

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                   
                    if(result != null)
                    {
                        return bool.TryParse(result, out bool res);
                    }
                    
                    return false;

                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<string> TestConnection()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"{_configuration.GetConnectionString("AuthService")}/TestConnection");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Sync GET to TestConnection was OK");
                    string message = await response.Content.ReadAsStringAsync();
                    return message;
                }
                else
                {
                    Console.WriteLine("Sync GET to TestConnection was FAILED");
                    return string.Empty;
                }
            }
        }
    }
}
