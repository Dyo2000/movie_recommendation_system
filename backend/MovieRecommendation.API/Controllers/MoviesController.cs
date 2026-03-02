using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        // Get all genres
        [HttpGet("genres")]
        public IActionResult GetGenres()
        {
            var genres = _context.Genres
                .Select(g => new { g.Id, g.Name, g.ExternalId })
                .ToList();

            return Ok(genres);
        }

        // Random movie, biased toward liked genres
        [HttpPost("random")]
        public IActionResult GetRandomMovie([FromBody] GenreFilter? filter)
        {
            var query = _context.Movies.AsQueryable();

            // Optional filter by genre IDs
            if (filter?.GenreIds != null && filter.GenreIds.Any())
                query = query.Where(m => filter.GenreIds.Contains(m.GenreId));

            var movies = query.ToList();
            if (!movies.Any())
                return NotFound("No movies found.");

            // Fetch liked genres for weighting (replace this with your actual tracking table)
            var likedGenreIds = new List<int>(); // TODO: populate from user preference table

            // Calculate weights for each movie
            var weightedMovies = movies.Select(m =>
            {
                int weight = likedGenreIds.Contains(m.GenreId) ? 5 : 1; // 5x weight if liked
                return new { Movie = m, Weight = weight };
            }).ToList();

            // Weighted random selection
            int totalWeight = weightedMovies.Sum(w => w.Weight);
            var rnd = new Random();
            int roll = rnd.Next(totalWeight);
            foreach (var w in weightedMovies)
            {
                if (roll < w.Weight)
                    return Ok(w.Movie);
                roll -= w.Weight;
            }

            // Fallback (should not hit)
            return Ok(weightedMovies.First().Movie);
        }

        // Get movies by genres (all matches)
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
    }
}