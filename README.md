# Movie Recommendation API

This is a .NET 8 Web API for managing and recommending movies based on genres. The API is built with **ASP.NET Core**, **Entity Framework Core**, and **SQLite**.

---

## Features

- Get all genres: `/api/Movies/genres`
- Get a random movie: `/api/Movies/random`  
  - Can filter by genre IDs (`genreIds`), optional.
- Get multiple movies by genre(s): `/api/Movies/by-genres`  
  - Currently requires at least one genre to be selected.
- Integration with TMDb to fetch real movie data.
- Swagger UI for testing endpoints.


---

## API Endpoints

| Method | Endpoint | Description | Body / Params |
|--------|----------|-------------|---------------|
| GET | `/api/Movies/genres` | Retrieve all genres | None |
| POST | `/api/Movies/random` | Returns a random movie based on optional genre filter | `{ "genreIds": [1, 2] }` |
| POST | `/api/Movies/by-genres` | Returns movies filtered by selected genres | `{ "genreIds": [1, 2] }` |

> If no genres are provided for the random movie endpoint, a completely random movie is returned.

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQLite (included via EF Core provider)
- Optional: Postman for API testing

### Run Locally

```bash
git clone <repo-url>
cd MovieRecommendation.API
dotnet build
dotnet run