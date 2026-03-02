using System.Text.Json;
using Microsoft.Extensions.Options;
using MovieRecommendation.API.Models.TMDb;
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

        /// <summary>
        /// Get list of all movie genres from TMDb
        /// </summary>
        public async Task<List<TMDbGenre>> GetGenresAsync()
        {
            var url = $"{_settings.BaseUrl}/genre/movie/list?api_key={_settings.ApiKey}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TMDbGenreResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.Genres ?? new List<TMDbGenre>();
        }

        /// <summary>
        /// Discover movies by genre IDs
        /// </summary>
        public async Task<List<TMDbMovie>> GetMoviesByGenresAsync(List<int> genreIds)
        {
            var genreQuery = string.Join(",", genreIds);

            var url = $"{_settings.BaseUrl}/discover/movie?api_key={_settings.ApiKey}&with_genres={genreQuery}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TMDbMovieResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.Results ?? new List<TMDbMovie>();
        }

        /// <summary>
        /// Get popular movies
        /// </summary>
        public async Task<List<TMDbMovie>> GetPopularMoviesAsync()
        {
            var url = $"{_settings.BaseUrl}/movie/popular?api_key={_settings.ApiKey}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TMDbMovieResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.Results ?? new List<TMDbMovie>();
        }
    }
}