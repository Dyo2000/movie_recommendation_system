# movie_recommendation_system

A full-stack movie recommendation system that suggests films by genre or random selection, with planned support for user profiles, reviews, and personalized recommendations.

## Project layout

- backend/MovieRecommendation.API — ASP.NET Core Web API
  - Controllers: [HealthController](backend/MovieRecommendation.API/Controllers/HealthController.cs)
  - Models: [`Movie`](backend/MovieRecommendation.API/Models/Movie.cs), [`Genre`](backend/MovieRecommendation.API/Models/Genre.cs)
  - Entry: [Program.cs](backend/MovieRecommendation.API/Program.cs)
- frontend — (frontend app folder)

## Features (current)
- Basic health endpoint: `GET /api/health` (see [`MovieRecommendation.API.Controllers.HealthController`](backend/MovieRecommendation.API/Controllers/HealthController.cs)).
- Placeholder weather demo endpoint remains in [Program.cs](backend/MovieRecommendation.API/Program.cs).
- Domain models: [`MovieRecommendation.API.Models.Movie`](backend/MovieRecommendation.API/Models/Movie.cs) and [`MovieRecommendation.API.Models.Genre`](backend/MovieRecommendation.API/Models/Genre.cs).

## Running the backend (development)
1. Ensure .NET SDK is installed (recommended .NET 7+).
2. From repository root, run:
```bash
cd 
dotnet build
dotnet run
```
