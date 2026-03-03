using System;
using System.Collections.Generic;
using System.Linq;
using MovieRecommendation.API.Models;

namespace MovieRecommendation.API.Data
{
    public static class DataSeeder
    {
        public static void Seed(MovieRecommendationContext context)
        {
            //if movies already exist, dont seed again
            if(context.Movies.Any())
            {
                return;
            }

            var action = new Genre { Name = "Action" };
            var comedy = new Genre { Name = "Comedy" };
            var horror = new Genre { Name = "Horror" };

            var movies = new List<Movie>
            {
                new Movie
                {
                    Title = "The Matrix",
                    Overview = "A computer hacker learns about the true nature of his reality and his role in the war against its controllers.",
                    PosterUrl = "https://image.tmdb.org/t/p/w500/f89U3ADr1oiB1s9GkdPOEpXUk5H.jpg",
                    Rating = 8.7,
                    Genre = action
                },
                new Movie
                {
                    Title = "The Hangover",
                    Overview = "Three buddies wake up from a bachelor party in Las Vegas, with no memory of the previous night and the bachelor missing.",
                    PosterUrl = "https://image.tmdb.org/t/p/w500/uluhlXubGu1VxU63X9d8r3qMsa.jpg",
                    Rating = 7.7,
                    Genre = comedy
                },
                new Movie
                {
                    Title = "The Conjuring",
                    Overview = "Paranormal investigators Ed and Lorraine Warren work to help a family terrorized by a dark presence in their farmhouse.",
                    PosterUrl = "https://image.tmdb.org/t/p/w500/4Y39QYtLqZz2iYFvRkUuGZllq.jpg",
                    Rating = 7.5,
                    Genre = horror
                }
            };
            context.Movies.AddRange(movies);
            context.SaveChanges();
        }
    }
}