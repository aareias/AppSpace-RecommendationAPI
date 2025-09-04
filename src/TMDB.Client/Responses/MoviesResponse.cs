namespace TMDB.Responses;

public class DiscoverMoviesResponse
{
    public IEnumerable<Movie> Results { get; set; } = new List<Movie>();

    public int TotalCount => Results.Count();

    public int TotalPages { get; set; }
}