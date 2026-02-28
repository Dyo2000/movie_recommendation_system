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
        public string? PosterPath { get; set; }
        public string? ReleaseDate { get; set; }
    }
}