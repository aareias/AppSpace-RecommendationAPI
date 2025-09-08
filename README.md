# AppSpace Recommendation API

This repository contains the AppSpace Recommendation API, a .NET solution for recommendations of all-time and upcoming content as well as intelligent billboard generation.

## Features
- RESTful API for recommendations and billboard generation
- Modular architecture with separate projects for API, Application logic, Domain, Data sources and TMDB client integration
- Unit and integration tests
- GitHub Actions workflow for CI/CD

## API Definition for the Exercise
See the full API specification in [API_Definition.md](API_Definition.md).

## Project Structure
```
AppSpace-RecommendationAPI.slnx         # Solution file
src/
  API/                                  # ASP.NET Core Web API
  Application/                          # Business logic and services
  SessionInformationDbSource/           # Data source configuration and repositories
  SessionsDB/                           # Database entities and repositories
  TMDB.Client/                          # TMDB API client
  Utils/                                # Utility classes
tests/
  Application.UnitTests/               # Unit tests
  TMDB.Client.Tests/                   # TMDB client tests
```

## Getting Started

### Prerequisites
- .NET 9 SDK
- (Optional) Visual Studio 2022 or VS Code

### Build and Run
1. Clone the repository:
   ```powershell
   git clone https://github.com/aareias/AppSpace-RecommendationAPI.git
   cd AppSpace-RecommendationAPI
   ```
2. Restore dependencies:
   ```powershell
   dotnet restore AppSpace-RecommendationAPI.slnx
   ```
3. Build the solution:
   ```powershell
   dotnet build AppSpace-RecommendationAPI.slnx --configuration Release
   ```
4. Run the API:
   ```powershell
   dotnet run --project src/API/API.csproj
   ```

### Run Tests
```powershell
dotnet test AppSpace-RecommendationAPI.slnx --configuration Release
```

## API Usage
See `src/API/API.http` for example requests.

## CI/CD
A GitHub Actions workflow builds and tests the solution on every merge to `master`. See `.github/workflows/build-and-test.yml`.
- Note: Currently not running due to my billing information in Github.
