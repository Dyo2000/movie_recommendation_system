namespace MovieRecommendation.API.Models
{
    public class Genre
    {
        public int Id { get; set; }

        //ExternalId is the ID from the external movie database
        public int ExternalId { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Movie> Movies { get; set; } = new();
    }
}