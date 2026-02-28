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

        // GET: /api/movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movies.Include(m => m.Genre).ToListAsync();
        }

        // GET: /api/movies/random
        [HttpGet("random")]
        public async Task<ActionResult<Movie>> GetRandomMovie()
        {
            var count = await _context.Movies.CountAsync();
            if (count == 0) return NotFound("No movies in database.");

            var index = new Random().Next(count);
            var movie = await _context.Movies
                .Include(m => m.Genre)
                .Skip(index)
                .FirstOrDefaultAsync();

            if (movie == null) 
                return NotFound();

            return movie;
        }

        // GET: /api/movies/by-genre/{genre}
        [HttpGet("by-genre/{genreName}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByGenre(string genreName)
        {
            var movies = await _context.Movies
                .Include(m => m.Genre)
                .Where(m => m.Genre != null && m.Genre.Name.ToLower() == genreName.ToLower())
                .ToListAsync();

            if (!movies.Any()) return NotFound($"No movies found for genre: {genreName}");

            return movies;
        }
    }
}