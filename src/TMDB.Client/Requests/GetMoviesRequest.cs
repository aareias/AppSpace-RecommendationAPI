namespace TMDB.Responses;

public class GetMoviesRequest
{
    public DateTime? ReleaseDateBeforeThan { get; set; } = null;
    public int WantedMovies { get; set; } = 0;
    public IEnumerable<int>? WithGenres { get; set; } = null;
    public IEnumerable<int>? WithoutGenres { get; set; } = null;
}