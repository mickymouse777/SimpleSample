using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleSample.Services
{
    public class TableStorageService
    {
        private readonly HttpClient _httpClient;

        public TableStorageService()
        {
            _httpClient = new HttpClient();
        }

        public async Task AddNameAsync(string name)
        {
            
            
            //var nameEntity = new NameEntity { Name = name };
            //await _tableClient.AddEntityAsync(nameEntity);
            

            var functionUrl = "https://mickgfunction.azurewebsites.net/api/AddNameFunction"; 

            var jsonPayload = JsonSerializer.Serialize(new { Name = name });
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(functionUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error adding name via function. Status Code: {response.StatusCode}");
            }
        }
    }
}
