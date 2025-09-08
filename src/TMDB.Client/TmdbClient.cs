using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Utils;
using TMDB.Client.Configurations;
using TMDB.Client.Requests;
using TMDB.Client.Responses;

namespace TMDB.Client;

public class TmdbClient(HttpClient client, ILogger<TmdbClient> logger) : ITmdbClient
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
        try
        {
            var query = new Dictionary<string, string?>
            {
                ["api_key"] = _apiKey
            };

            var finalUri = QueryHelpers.AddQueryString($"{_baseUrl}/{_apiVersion}/genre/movie/list", query);

            var response = await client.GetAsync(finalUri, ct);
            
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync(ct);
    
            var result = await JsonSerializer.DeserializeAsync<GenresResponse>(
                stream,
                _options,
                ct
            );

            if (result == null || !result.Genres.Any())
            {
                throw new InvalidDataException("Failed to deserialize response from TMDB.");
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch movie genres from TMDB.");
            throw;
        }
    }

    public async Task<IEnumerable<MovieResponse>> GetMoviesAsync(GetMoviesRequest request, CancellationToken ct = default)
    {
        List<MovieResponse> movieList = new();

        int totalPages = 1;
        int page = 1;

        var query = BuildDiscoverMovieQueryParameters(request);

        try
        {
            while (movieList.Count < request.WantedMovies && page <= totalPages)
            {
                query["page"] = page.ToString();

                var finalUri = QueryHelpers.AddQueryString($"{_baseUrl}/{_apiVersion}/discover/movie", query);

                var response = await client.GetAsync(finalUri, ct);
            
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync(ct);
    
                var result = await JsonSerializer.DeserializeAsync<DiscoverMoviesResponse>(
                    stream,
                    _options,
                    ct
                );

                if (result == null || !result.Results.Any())
                {
                    throw new InvalidDataException("Failed to parse response from TMDB discover movie.");
                }

                movieList.AddRange(result.Results);

                page++;
                totalPages = result.TotalPages;
            }

            return movieList;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch movies from TMDB.");
            throw;
        }
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
            query.Add("without_genres", string.Join(" AND ", request.WithoutGenres));
        }

        return query;
    }
}