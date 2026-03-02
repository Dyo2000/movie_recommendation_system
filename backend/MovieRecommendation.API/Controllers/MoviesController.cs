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

        [HttpGet("genres")]
        public IActionResult GetGenres()
        {
            var genres = _context.Genres
                .Select(g => new { g.Id, g.Name, g.ExternalId })
                .ToList();

            return Ok(genres);
        }

        [HttpPost("random")]
        public IActionResult GetRandomMovie([FromBody] GenreFilter? filter)
        {
            var query = _context.Movies.AsQueryable();

            if (filter?.GenreIds != null && filter.GenreIds.Any())
                query = query.Where(m => filter.GenreIds.Contains(m.GenreId));

            var movies = query.ToList();
            if (!movies.Any())
                return NotFound("No movies found.");

            var rnd = new Random();
            return Ok(movies[rnd.Next(movies.Count)]);
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
    }
}