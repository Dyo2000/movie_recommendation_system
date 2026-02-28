# Movie Recommendation API

This is a .NET 8 Web API for managing and recommending movies based on genres. The API is built with **ASP.NET Core**, **Entity Framework Core**, and **SQLite**.

---

## Features

- Retrieve all genres.
- Get a random movie with optional genre filtering.
- Get multiple movies filtered by selected genres.
- Fully documented with **Swagger UI**.
- Seeded database with sample movies and genres.

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