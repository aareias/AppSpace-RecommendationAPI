using SessionsDB.Entities;

namespace SessionsDB.Repositories.Abstractions;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetMostSuccessfulMoviesAsync();
}
