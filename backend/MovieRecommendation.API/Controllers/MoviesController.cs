using Microsoft.AspNetCore.Mvc;
using MovieRecommendation.API.Data;
using MovieRecommendation.API.Models;
using MovieRecommendation.API.Services;

namespace MovieRecommendation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieRecommendationContext _context;
        private readonly TmdbService _tmdbService;

        public MoviesController(
            MovieRecommendationContext context,
            TmdbService tmdbService)
        {
            _context = context;
            _tmdbService = tmdbService;
        }

        [HttpGet("discover")]
        public async Task<IActionResult> Discover()
        {
            var tmdbMovies = await _tmdbService.GetPopularMoviesAsync();

            await _tmdbService.SyncMoviesToDatabaseAsync(tmdbMovies);

            var localMovies = _context.Movies
                .OrderByDescending(m => m.Id)
                .Take(20)
                .ToList();

            return Ok(localMovies);
        }

        // Get all genres
        [HttpGet("genres")]
        public IActionResult GetGenres()
        {
            var genres = _context.Genres
                .Select(g => new { g.Id, g.Name, g.ExternalId })
                .ToList();

            return Ok(genres);
        }

        // Weighted random movie
        [HttpPost("random")]
        public IActionResult GetRandomMovie([FromBody] GenreFilter? filter)
        {
            var query = _context.Movies.AsQueryable();

            if (filter?.GenreIds != null && filter.GenreIds.Any())
                query = query.Where(m => filter.GenreIds.Contains(m.GenreId));

            var dislikedMovieIds = _context.UserMoviePreferences
                .Where(p => !p.IsLiked)
                .Select(p => p.MovieId)
                .ToList();

            query = query.Where(m => !dislikedMovieIds.Contains(m.Id));

            var movies = query.ToList();
            if (!movies.Any())
                return NotFound("No movies available.");

            var likedGenreIds = _context.UserMoviePreferences
                .Where(p => p.IsLiked)
                .Select(p => p.Movie!.GenreId)
                .ToList();

            var weightedMovies = movies.Select(m =>
            {
                int weight = likedGenreIds.Contains(m.GenreId) ? 5 : 1;
                return new { Movie = m, Weight = weight };
            }).ToList();

            int totalWeight = weightedMovies.Sum(w => w.Weight);
            var rnd = new Random();
            int roll = rnd.Next(totalWeight);

            foreach (var w in weightedMovies)
            {
                if (roll < w.Weight)
                    return Ok(w.Movie);

                roll -= w.Weight;
            }

            return Ok(weightedMovies.First().Movie);
        }

        [HttpPost("by-genres")]
        public IActionResult GetByGenres([FromBody] GenreFilter filter)
        {
            if (filter?.GenreIds == null || !filter.GenreIds.Any())
                return BadRequest("At least one genre must be selected.");

            var movies = _context.Movies
                .Where(m => filter.GenreIds.Contains(m.GenreId))
                .ToList();

            return Ok(movies);
        }

        [HttpPost("like")]
        public IActionResult LikeMovie([FromBody] int movieId)
        {
            if (!_context.Movies.Any(m => m.Id == movieId))
                return BadRequest("Movie does not exist.");

            var existing = _context.UserMoviePreferences
                .FirstOrDefault(p => p.MovieId == movieId && p.UserId == 1);

            if (existing != null)
                existing.IsLiked = true;
            else
                _context.UserMoviePreferences.Add(new UserMoviePreference
                {
                    MovieId = movieId,
                    UserId = 1,
                    IsLiked = true
                });

            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("dislike")]
        public IActionResult DislikeMovie([FromBody] int movieId)
        {
            if (!_context.Movies.Any(m => m.Id == movieId))
                return BadRequest("Movie does not exist.");

            var existing = _context.UserMoviePreferences
                .FirstOrDefault(p => p.MovieId == movieId && p.UserId == 1);

            if (existing != null)
                existing.IsLiked = false;
            else
                _context.UserMoviePreferences.Add(new UserMoviePreference
                {
                    MovieId = movieId,
                    UserId = 1,
                    IsLiked = false
                });

            _context.SaveChanges();
            return Ok();
        }
    }
}