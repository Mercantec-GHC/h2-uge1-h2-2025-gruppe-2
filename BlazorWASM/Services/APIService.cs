using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorWASM.Services
{
    public class APIService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://opgaver.mercantec.tech/api";
        private const string BasicUrl = "https://opgaver.mercantec.tech";

        public APIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BackendStatus?> GetBackendStatusAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/Status/all");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<BackendStatus>(jsonString, options);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved tjek af backend status: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Miles>> GetMilesPricesAsync()
        {
            List<Miles> prices = [];
            
            try
            {
                var response = await _httpClient.GetAsync($"{BasicUrl}/Opgaver/Miles95");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    options.Converters.Add(new StringToDoubleConverter());
                    prices = JsonSerializer.Deserialize<List<Miles>>(json, options) ?? throw new NullReferenceException();
                    if (prices.Count > 0)
                        Console.WriteLine(prices[0].Price + " " + prices[0].Date);
                }
                else
                {
                    Console.WriteLine($"Error getting miles and dates: {response.StatusCode}");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting miles and dates: {ex.Message}");
            }
            
            return prices;
        }
    }

    public class BackendStatus
    {
        public ServerStatus? Server { get; set; }
        public DatabaseStatus? MongoDB { get; set; }
        public DatabaseStatus? PostgreSQL { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class ServerStatus
    {
        public string Status { get; set; } = string.Empty;
    }

    public class DatabaseStatus
    {
        public string Status { get; set; } = string.Empty;
        public string? Database { get; set; }
        public string? Error { get; set; }
        public bool IsError { get; set; }
    }

}
