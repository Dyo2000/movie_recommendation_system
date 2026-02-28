using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRecommendation.API.Data;
using MovieRecommendation.API.Models;

namespace MovieRecommendation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieRecommendationContext _context;

        public MoviesController(MovieRecommendationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all genres in the system.
        /// </summary>
        /// <returns>List of genres</returns>
        [HttpGet("genres")]
        [ProducesResponseType(typeof(IEnumerable<Genre>), 200)]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            var genres = await _context.Genres.Include(g => g.Movies).ToListAsync();
            return Ok(genres);
        }

        /// <summary>
        /// Returns a random movie based on selected genres.
        /// If no genres are provided, returns a completely random movie.
        /// </summary>
        /// <param name="filter">Optional list of genre IDs</param>
        /// <returns>A random movie</returns>
        [HttpPost("random")]
        [ProducesResponseType(typeof(Movie), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Movie>> GetRandomMovie([FromBody] GenreFilter? filter)
        {
            IQueryable<Movie> query = _context.Movies.Include(m => m.Genre);

            if (filter?.GenreIds != null && filter.GenreIds.Any())
            {
                query = query.Where(m => filter.GenreIds.Contains(m.GenreId));
            }

            var count = await query.CountAsync();
            if (count == 0) return NotFound("No movies found for the selected genres.");

            var index = new Random().Next(count);
            var movie = await query.Skip(index).FirstOrDefaultAsync();

            if (movie == null) return NotFound();

            return movie;
        }

        /// <summary>
        /// Returns multiple movies based on selected genres.
        /// If no genres are provided, returns all movies (optional: could limit results).
        /// </summary>
        /// <param name="filter">Optional list of genre IDs</param>
        /// <returns>List of movies</returns>
        [HttpPost("by-genres")]
        [ProducesResponseType(typeof(IEnumerable<Movie>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByGenres([FromBody] GenreFilter? filter)
        {
            IQueryable<Movie> query = _context.Movies.Include(m => m.Genre);

            if (filter?.GenreIds != null && filter.GenreIds.Any())
            {
                query = query.Where(m => filter.GenreIds.Contains(m.GenreId));
            }

            var movies = await query.ToListAsync();

            if (!movies.Any()) return NotFound("No movies found for the selected genres.");

            return movies;
        }
    }

    /// <summary>
    /// Filter object for movie selection based on genres.
    /// </summary>
    public class GenreFilter
    {
        /// <summary>
        /// Optional list of genre IDs to filter the movies
        /// </summary>
        public List<int>? GenreIds { get; set; }
    }
}