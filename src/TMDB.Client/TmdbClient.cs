using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Utils;
using TMDB.Configurations;
using TMDB.Responses;

namespace TMDB;

public class TmdbClient(HttpClient client) : ITmdbClient
{
    readonly string _baseUrl = ApplicationSettings.GetValue<string>(TMDBApplicationConstants.TMDBBaseUrl);
    readonly string _apiVersion = ApplicationSettings.GetValue<string>(TMDBApplicationConstants.TMDBApiVersion);
    readonly string _apiKey = ApplicationSettings.GetValue<string>(TMDBApplicationConstants.TMDBApiKey);
    
    readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<GenresResponse> GetMovieGenresAsync(CancellationToken ct)
    {
        var query = new Dictionary<string, string?>
        {
            ["api_key"] = _apiKey
        };

        var finalUri = QueryHelpers.AddQueryString($"{_baseUrl}/{_apiVersion}/genre/movie/list", query);

        var response = await client.GetStringAsync(finalUri, ct);

        var parsedResult = JsonSerializer.Deserialize<GenresResponse>(response, _options);

        if (parsedResult == null)
        {
            throw new Exception("Failed to fetch genres from TMDB.");
        }

        return parsedResult;
    }

    public async Task<IEnumerable<Movie>> GetMoviesAsync(GetMoviesRequest request, CancellationToken ct = default)
    {
        List<Movie> movieList = new();

        int totalPages = 1;
        int page = 1;

        var query = BuildDiscoverMovieQueryParameters(request);

        while (movieList.Count < request.WantedMovies && page <= totalPages)
        {
            query["page"] = page.ToString();

            var finalUri = QueryHelpers.AddQueryString($"{_baseUrl}/{_apiVersion}/discover/movie", query);

            var response = await client.GetStringAsync(finalUri, ct);

            var result = JsonSerializer.Deserialize<DiscoverMoviesResponse>(response, _options);

            if (result == null)
            {
                throw new Exception("Failed to fetch movies from TMDB.");
            }

            movieList.AddRange(result.Results ?? Enumerable.Empty<Movie>());

            page++;
            totalPages = result.TotalPages;
        }

        return movieList.Take(request.WantedMovies);
    }

    Dictionary<string, string?> BuildDiscoverMovieQueryParameters(GetMoviesRequest request)
    {
        var query = new Dictionary<string, string?>
        {
            ["api_key"] = _apiKey,
            ["include_video"] = "false",
            ["sort_by"] = "popularity.desc"
        };

        if (request.ReleaseDateBeforeThan != null)
        {
            query.Add("release_date.lte", request.ReleaseDateBeforeThan.Value.ToString("yyyy-MM-dd"));
        }

        if (request.WithGenres != null && request.WithGenres.Any())
        {
            query.Add("with_genres", string.Join(" OR ", request.WithGenres));
        }

        if (request.WithoutGenres != null && request.WithoutGenres.Any())
        {
            query.Add("without_genres", string.Join(" OR ", request.WithoutGenres));
        }

        return query;
    }
}