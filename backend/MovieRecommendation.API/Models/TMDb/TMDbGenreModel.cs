using System.Collections.Generic;

namespace MovieRecommendation.API.Models.TMDb
{
    // Represents a single genre from TMDb
    public class TMDbGenre
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // Response wrapper for /genre/movie/list endpoint
    public class TMDbGenreResponse
    {
        public List<TMDbGenre> Genres { get; set; } = new();
    }
}