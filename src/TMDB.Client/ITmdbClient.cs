using TMDB.Responses;

namespace TMDB;

public interface ITmdbClient
{
    Task<GenresResponse> GetMovieGenresAsync(CancellationToken ct);

    Task<IEnumerable<Movie>> GetMoviesAsync(GetMoviesRequest request, CancellationToken ct);
}