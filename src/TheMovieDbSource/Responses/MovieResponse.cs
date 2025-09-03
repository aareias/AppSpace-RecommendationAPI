using System.Text.Json.Serialization;

namespace TheMovieDbSource.Responses;

public class Movie
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }
    
    [JsonPropertyName("genre_ids")]
    public IEnumerable<int> GenreIds { get; set; }
    
    [JsonPropertyName("original_title")]
    public string OriginalTitle { get; set; }
    
    [JsonPropertyName("overview")]
    public string Overview { get; set; }
    
    [JsonPropertyName("popularity")]
    public decimal Popularity { get; set; }
    
    [JsonPropertyName("release_date")]
    public DateTime ReleaseDate { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("original_language")]
    public string OriginalLanguage { get; set; }
    
    [JsonPropertyName("vote_average")]
    public decimal VoteAverage { get; set; }
    
    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
}