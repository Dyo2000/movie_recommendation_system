using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MovieRecommendation.API.Data;
using MovieRecommendation.API.Models;

namespace MovieRecommendation.API.Services
{
    public class TmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly MovieRecommendationContext _context;

        public TmdbService(
            HttpClient httpClient,
            IConfiguration configuration,
            MovieRecommendationContext context)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
        }

        // Fetch popular movies from TMDb
        public async Task<List<TmdbMovieDto>> GetPopularMoviesAsync()
        {
            var apiKey = _configuration["TMDB:ApiKey"];
            var url = $"https://api.themoviedb.org/3/movie/popular?api_key={apiKey}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TmdbResponseDto>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.Results ?? new List<TmdbMovieDto>();
        }

        // Fetch genres from TMDb
        public async Task<List<TmdbGenreDto>> GetGenresAsync()
        {
            var apiKey = _configuration["TMDB:ApiKey"];
            var url = $"https://api.themoviedb.org/3/genre/movie/list?api_key={apiKey}&language=en-US";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TmdbGenreResponseDto>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.Genres ?? new List<TmdbGenreDto>();
        }

        // Fetch movies by TMDb genre IDs
        public async Task<List<TmdbMovieDto>> GetMoviesByGenresAsync(List<int> genreIds)
        {
            var apiKey = _configuration["TMDB:ApiKey"];
            var genreParam = string.Join(",", genreIds);
            var url = $"https://api.themoviedb.org/3/discover/movie?api_key={apiKey}&with_genres={genreParam}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TmdbResponseDto>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.Results ?? new List<TmdbMovieDto>();
        }

        // Sync TMDb movies into local database
        public async Task SyncMoviesToDatabaseAsync(List<TmdbMovieDto> tmdbMovies)
        {
            foreach (var tmdbMovie in tmdbMovies)
            {
                var existingMovie = await _context.Movies
                    .FirstOrDefaultAsync(m => m.ExternalId == tmdbMovie.Id);

                if (existingMovie != null)
                    continue;

                // For simplicity, just take the first genre that exists locally
                var localGenre = await _context.Genres
                    .FirstOrDefaultAsync(g => tmdbMovie.GenreIds.Contains(g.ExternalId));

                if (localGenre == null)
                    continue;

            var newMovie = new Movie
            {
                ExternalId = tmdbMovie.Id,
                Title = tmdbMovie.Title ?? string.Empty,
                Overview = tmdbMovie.Overview ?? string.Empty,
                ReleaseDate = tmdbMovie.ReleaseDate,
                GenreId = localGenre.Id,
                PosterUrl = tmdbMovie.PosterPath ?? string.Empty,
                Rating = tmdbMovie.VoteAverage
            };

                _context.Movies.Add(newMovie);
            }

            await _context.SaveChangesAsync();
        }
    }

    // DTOs
    public class TmdbResponseDto
    {
        public List<TmdbMovieDto> Results { get; set; } = new();
    }

    public class TmdbMovieDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Overview { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public List<int> GenreIds { get; set; } = new();
        public string? PosterPath { get; set; }
        public double? VoteAverage { get; set; }
    }

    public class TmdbGenreResponseDto
    {
        public List<TmdbGenreDto> Genres { get; set; } = new();
    }

    public class TmdbGenreDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}