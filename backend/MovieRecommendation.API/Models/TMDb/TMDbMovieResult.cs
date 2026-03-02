using System.Collections.Generic;

namespace MovieRecommendation.API.Models.TMDb
{
    // Represents a paginated response from TMDb's API
    public class TMDbMovieResult
    {
        public int Page { get; set; }

        // List of movies returned in this page
        public List<TMDbMovie>? Results { get; set; }
    }

    // Represents a single movie from TMDb
    public class TMDbMovie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public string? PosterPath { get; set; }
        public string? ReleaseDate { get; set; }
    }
}