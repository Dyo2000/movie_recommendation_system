using Microsoft.AspNetCore.Mvc;
using MovieRecommendation.API.Models;
using MovieRecommendation.API.Services;

namespace MovieRecommendation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TMDbController : ControllerBase
    {
        private readonly TMDbService _tmdbService;

        public TMDbController(TMDbService tmdbService)
        {
            _tmdbService = tmdbService;
        }

        /// <summary>
        /// Get all available movie genres from TMDb
        /// </summary>
        [HttpGet("genres")]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _tmdbService.GetGenresAsync();
            return Ok(genres);
        }

        /// <summary>
        /// Discover movies based on selected TMDb genre IDs
        /// Example body: { "genreIds": [28, 35] }
        /// </summary>
        [HttpPost("discover")]
        public async Task<IActionResult> DiscoverMovies([FromBody] GenreFilter filter)
        {
            if (filter == null || filter.GenreIds == null || !filter.GenreIds.Any())
                return BadRequest("At least one genre must be selected.");

            var movies = await _tmdbService.GetMoviesByGenresAsync(filter.GenreIds);

            return Ok(movies);
        }

        /// <summary>
        /// Surprise me — returns one random popular movie
        /// </summary>
        [HttpGet("random")]
        public async Task<IActionResult> GetRandomMovie()
        {
            var movies = await _tmdbService.GetPopularMoviesAsync();

            if (movies == null || !movies.Any())
                return NotFound("No movies found.");

            var random = new Random();
            var movie = movies[random.Next(movies.Count)];

            return Ok(movie);
        }
    }
}