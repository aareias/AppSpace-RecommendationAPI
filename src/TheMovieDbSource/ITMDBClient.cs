using TheMovieDbSource.Responses;

namespace TheMovieDbSource;

public interface ITMDBClient
{
    Task<GenresResponse?> GetGenresForType(MediaType mediaType, CancellationToken ct);
}