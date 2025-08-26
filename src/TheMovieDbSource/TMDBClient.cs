using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Shared;
using TheMovieDbSource.Responses;

namespace TheMovieDbSource;

public class TMDBClient(HttpClient client) : ITMDBClient
{
    readonly string _baseUrl = ApplicationSettings.GetValue(TMDBApplicationConstants.TMDBBaseUrl);
    readonly string _apiVersion = ApplicationSettings.GetValue(TMDBApplicationConstants.TMDBApiVersion);
    readonly string _apiKey = ApplicationSettings.GetValue(TMDBApplicationConstants.TMDBApiKey);
    
    readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };
    
    public async Task<GenresResponse?> GetGenresForType(MediaType mediaType, CancellationToken ct)
    {
        var query = new Dictionary<string, string?>
        {
            ["api_key"] = _apiKey
        };

        var mediaTypeRoute = GetRouteFromMediaType(mediaType);
        
        string finalUri = QueryHelpers.AddQueryString($"{_baseUrl}/{_apiVersion}/genre/{mediaTypeRoute}/list", query);
        
        var response = await client.GetStringAsync(finalUri, ct);

        return JsonSerializer.Deserialize<GenresResponse>(response, _options);
    }

    static string GetRouteFromMediaType(MediaType mediaType)
    {
        return mediaType switch
        {
            MediaType.Movie => "movie",
            MediaType.Tv => "tv",
            _ => throw new ArgumentOutOfRangeException(nameof(mediaType), mediaType, null)
        };
    }
}