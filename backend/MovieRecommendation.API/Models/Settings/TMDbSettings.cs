namespace MovieRecommendation.API.Models.Settings
{
    public class TMDbSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        
        public string BaseUrl { get; set; } = "https://api.themoviedb.org/3/"; // default TMDb API base
    }
}