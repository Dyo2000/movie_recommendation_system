namespace MovieRecommendation.API.Models
{
    public class Movie
    {
        public int Id { get; set; }

        // External API movie id
        public int ExternalId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Overview { get; set; }

        public int ReleaseYear { get; set; }

        public string? PosterUrl { get; set; }

        public double Rating { get; set; }

        // Relationship to Genre
        public int GenreId { get; set; }

        public Genre? Genre { get; set; }
    }
}