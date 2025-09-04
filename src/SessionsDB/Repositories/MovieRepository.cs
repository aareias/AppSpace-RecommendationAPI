using Microsoft.EntityFrameworkCore;
using SessionsDB.Configuration;
using SessionsDB.Entities;
using SessionsDB.Repositories.Abstractions;

namespace SessionsDB.Repositories;

public class MovieRepository(SessionContext Context) : IMovieRepository
{
    readonly DbSet<Movie> _dbSet = Context.Set<Movie>();

    public async Task<IEnumerable<Movie>> GetMostSuccessfulMoviesAsync()
    {
        return await _dbSet
            .Include(m => m.Genres)
            .Include(m => m.Sessions)
            .OrderByDescending(m => m.Sessions.Sum(s => s.SeatsSold))
            .ToListAsync();
    }
}