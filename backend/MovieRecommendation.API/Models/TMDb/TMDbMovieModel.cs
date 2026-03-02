using System.Text.Json.Serialization;

namespace MovieRecommendation.API.Models.TMDb
{
    public class TMDbMovieResult
    {
        public int Page { get; set; }
        public List<TMDbMovie>? Results { get; set; }
    }

    public class TMDbMovie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }
    }
}