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

        /// <summary>
        /// Get movies from TMDb filtered by genre IDs.
        /// Example: Action (28), Comedy (35)
        /// </summary>
        public async Task<List<TMDbMovieDto>> GetMoviesByGenresAsync(List<int> genreIds, int page = 1)
        {
            if (genreIds == null || !genreIds.Any())
                return new List<TMDbMovieDto>();

            var genreString = string.Join(",", genreIds);

            var url =
                $"{_settings.BaseUrl}/discover/movie" +
                $"?api_key={_settings.ApiKey}" +
                $"&with_genres={genreString}" +
                $"&page={page}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<TMDbMovieDto>();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<TMDbDiscoverResponse>(json, options);

            return result?.Results ?? new List<TMDbMovieDto>();
        }

        /// <summary>
        /// Optional: Get popular movies (useful for testing or "Surprise Me")
        /// </summary>
        public async Task<List<TMDbMovieDto>> GetPopularMoviesAsync(int page = 1)
        {
            var url =
                $"{_settings.BaseUrl}/movie/popular" +
                $"?api_key={_settings.ApiKey}" +
                $"&page={page}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<TMDbMovieDto>();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<TMDbDiscoverResponse>(json, options);

            return result?.Results ?? new List<TMDbMovieDto>();
        }
    }

    /// <summary>
    /// Wrapper for TMDb discover/popular responses
    /// </summary>
    public class TMDbDiscoverResponse
    {
        public int Page { get; set; }
        public List<TMDbMovieDto> Results { get; set; } = new();
    }

    /// <summary>
    /// Simplified movie object returned from TMDb
    /// </summary>
    public class TMDbMovieDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Overview { get; set; }
        public string? Poster_Path { get; set; }
        public string? Release_Date { get; set; }
        public double Vote_Average { get; set; }
        public List<int>? Genre_Ids { get; set; }
    }
}