using System.Collections.Generic;

namespace MovieRecommendation.API.Models.TMDb
{
    public class TMDbGenreResponse
    {
        public List<TMDbGenre> Genres { get; set; } = new();
    }
}