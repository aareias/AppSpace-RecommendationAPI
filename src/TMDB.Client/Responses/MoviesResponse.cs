using System.Text.Json.Serialization;

namespace TMDB.Client.Responses;

public class DiscoverMoviesResponse
{
    public IEnumerable<MovieResponse> Results { get; set; } = new List<MovieResponse>();

    public int TotalCount => Results.Count();

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }
}