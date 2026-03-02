namespace MovieRecommendation.API.Models
{
    public class MovieFeedback
    {
        public int Id { get; set; }

        // TMDb movie ID
        public int ExternalMovieId { get; set; }

        // true = liked, false = disliked
        public bool Liked { get; set; }
    }
}