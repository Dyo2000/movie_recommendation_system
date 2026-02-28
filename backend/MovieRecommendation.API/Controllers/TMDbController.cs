using Microsoft.AspNetCore.Mvc;
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
        /// Get popular movies from TMDb
        /// </summary>
        [HttpGet("popular")]
        public async Task<IActionResult> GetPopular()
        {
            var movies = await _tmdbService.GetPopularMoviesAsync();
            if (movies == null) return NotFound("No movies found.");
            return Ok(movies);
        }
    }
}