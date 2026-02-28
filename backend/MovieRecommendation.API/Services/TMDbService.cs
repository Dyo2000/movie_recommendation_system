using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MovieRecommendation.API.Models.Settings;

namespace MovieRecommendation.API.Services
{
    public class TMDbService
    {
        private readonly HttpClient _httpClient;
        private readonly TMDbSettings _settings;

        public TMDbService(HttpClient httpClient, IOptions<TMDbSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<string?> GetPopularMoviesAsync()
        {
            var url = $"{_settings.BaseUrl}/movie/popular?api_key={_settings.ApiKey}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsStringAsync();
        }
    }
}