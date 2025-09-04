using System.Text.Json.Serialization;

namespace TMDB.Responses;

public class Movie
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }
    
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }
    
    [JsonPropertyName("genre_ids")]
    public required IEnumerable<int> GenreIds { get; set; }
    
    [JsonPropertyName("original_title")]
    public required string OriginalTitle { get; set; }
    
    [JsonPropertyName("overview")]
    public required string Overview { get; set; }
    
    [JsonPropertyName("popularity")]
    public decimal Popularity { get; set; }
    
    [JsonPropertyName("release_date")]
    public DateTime ReleaseDate { get; set; }
    
    [JsonPropertyName("title")]
    public required string Title { get; set; }
    
    [JsonPropertyName("original_language")]
    public required string OriginalLanguage { get; set; }
    
    [JsonPropertyName("vote_average")]
    public decimal VoteAverage { get; set; }
    
    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
}