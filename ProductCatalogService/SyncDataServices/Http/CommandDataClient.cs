
using Microsoft.Extensions.Configuration;

namespace ProductCatalogService.SyncDataServices.Http
{
    public class CommandDataClient : ICommandDataClient
    {
        private readonly IConfiguration _configuration;

        public CommandDataClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetId(string token)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"{_configuration.GetConnectionString("AuthService")}/GetUserId?token={token}");
                Console.WriteLine($"{_configuration.GetConnectionString("AuthService")}/GetUserId?token={token}");
                if (response.IsSuccessStatusCode)
                {
                    string id = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Sync GET to AuthService was OK");
                    return id;
                }
                else
                {
                    Console.WriteLine("Sync GET to AuthService was FAILED");
                    return string.Empty;
                }
            }
        }
    }
}
