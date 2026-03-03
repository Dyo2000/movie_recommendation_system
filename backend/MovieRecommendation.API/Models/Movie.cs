using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRecommendation.API.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        public int ExternalId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Overview { get; set; } = string.Empty;

        // TMDb release date
        public DateTime? ReleaseDate { get; set; }

        // TMDb release year (optional convenience)
        public int? ReleaseYear => ReleaseDate?.Year;

        // TMDb poster URL
        public string PosterUrl { get; set; } = string.Empty;

        // TMDb rating (vote_average)
        public double? Rating { get; set; }

        // Foreign key for Genre
        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
    }
}