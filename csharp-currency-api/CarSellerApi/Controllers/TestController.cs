using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CarSellerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CurrencyController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> GetRates()
        {
            // External API (free one)
            var url = "https://api.exchangerate-api.com/v4/latest/USD";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(500, "Error calling external API");
            }

            var json = await response.Content.ReadAsStringAsync();

            // Parse JSON
            var data = JsonSerializer.Deserialize<ExchangeResponse>(json);

            // Format output (your custom response)
            var result = new
            {
                Base = data.Base,
                EUR = data.rates["EUR"],
                GBP = data.rates["GBP"],
                AMD = data.rates["AMD"]
            };

            return Ok(result);
        }
    }

    public class ExchangeResponse
    {
        [JsonPropertyName("base")]
        public string Base { get; set; }

        public Dictionary<string, decimal> rates { get; set; }
    }
}