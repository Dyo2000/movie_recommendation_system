using Microsoft.EntityFrameworkCore;
using MovieRecommendation.API.Models;

namespace MovieRecommendation.API.Data
{
    public class MovieRecommendationContext : DbContext
    {
        public MovieRecommendationContext(DbContextOptions<MovieRecommendationContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;
        
        public DbSet<Genre> Genres { get; set; } = null!;

        public DbSet<MovieFeedback> MovieFeedbacks { get; set; } = null!;

        public DbSet<UserMoviePreference> UserMoviePreferences { get; set; }

    }
}