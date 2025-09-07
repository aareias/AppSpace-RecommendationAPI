using TMDB.Client.Requests;
using TMDB.Client.Responses;

namespace TMDB.Client;

public interface ITmdbClient
{
    Task<GenresResponse> GetMovieGenresAsync(CancellationToken ct);

    Task<IEnumerable<MovieResponse>> GetMoviesAsync(GetMoviesRequest request, CancellationToken ct);
}