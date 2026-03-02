namespace MovieRecommendation.API.Models
{
    public class UserMoviePreference
    {
        public int Id { get; set; }

        // Movie this preference is tied to
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        // Could extend to actual user later
        public int UserId { get; set; } = 1; 

        // True if liked, false if disliked
        public bool IsLiked { get; set; }

        // Optional timestamp
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}