using Microsoft.AspNetCore.Mvc;
using MovieRecommendation.API.Services;
using MovieRecommendation.API.Models;

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
        /// Get movies from TMDb based on selected genre IDs.
        /// Example: [28, 35] for Action + Comedy
        /// </summary>
        /// <param name="filter">List of TMDb genre IDs</param>
        /// <returns>List of movies from TMDb</returns>
        [HttpPost("discover")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DiscoverMovies([FromBody] GenreFilter filter)
        {
            if (filter == null || filter.GenreIds == null || !filter.GenreIds.Any())
                return BadRequest("At least one genre must be selected.");

            var movies = await _tmdbService.GetMoviesByGenresAsync(filter.GenreIds);

            return Ok(movies);
        }


        /// <summary>
        /// Returns ONE random movie based on selected TMDb genre IDs.
        /// </summary>
        [HttpPost("random")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRandomMovie([FromBody] GenreFilter filter)
        {
            if (filter == null || filter.GenreIds == null || !filter.GenreIds.Any())
                return BadRequest("At least one genre must be selected.");

            var movies = await _tmdbService.GetMoviesByGenresAsync(filter.GenreIds);

            if (movies == null || !movies.Any())
                return NotFound("No movies found for the selected genres.");

            var random = new Random();
            var movie = movies[random.Next(movies.Count)];

            return Ok(movie); // single movie
        }
    }
}