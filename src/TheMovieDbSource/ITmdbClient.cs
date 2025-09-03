using TheMovieDbSource.Responses;

namespace TheMovieDbSource;

public interface ITmdbClient
{
    Task<GenresResponse> GetMovieGenresAsync(CancellationToken ct);

    Task<IEnumerable<Movie>> GetMoviesAsync(GetMoviesRequest request, CancellationToken ct);
}