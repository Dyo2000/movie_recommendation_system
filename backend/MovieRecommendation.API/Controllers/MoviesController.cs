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

        // =========================
        // GENRES
        // =========================
        [HttpGet("genres")]
        public IActionResult GetGenres()
        {
            var genres = _context.Genres
                .Select(g => new { g.Id, g.Name, g.ExternalId })
                .ToList();

            return Ok(genres);
        }

        // =========================
        // RANDOM MOVIE (from local DB)
        // =========================
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

        // =========================
        // GET ALL MOVIES BY GENRES
        // =========================
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

        // =========================
        // LIKE MOVIE
        // =========================
        [HttpPost("like")]
        public IActionResult LikeMovie([FromBody] FeedbackRequest request)
        {
            if (request == null || request.ExternalMovieId <= 0)
                return BadRequest("Invalid movie id.");

            var existing = _context.MovieFeedbacks
                .FirstOrDefault(x => x.ExternalMovieId == request.ExternalMovieId);

            if (existing != null)
            {
                existing.Liked = true;
            }
            else
            {
                _context.MovieFeedbacks.Add(new MovieFeedback
                {
                    ExternalMovieId = request.ExternalMovieId,
                    Liked = true
                });
            }

            _context.SaveChanges();
            return Ok("Movie liked.");
        }

        // =========================
        // DISLIKE MOVIE
        // =========================
        [HttpPost("dislike")]
        public IActionResult DislikeMovie([FromBody] FeedbackRequest request)
        {
            if (request == null || request.ExternalMovieId <= 0)
                return BadRequest("Invalid movie id.");

            var existing = _context.MovieFeedbacks
                .FirstOrDefault(x => x.ExternalMovieId == request.ExternalMovieId);

            if (existing != null)
            {
                existing.Liked = false;
            }
            else
            {
                _context.MovieFeedbacks.Add(new MovieFeedback
                {
                    ExternalMovieId = request.ExternalMovieId,
                    Liked = false
                });
            }

            _context.SaveChanges();
            return Ok("Movie disliked.");
        }
    }
}