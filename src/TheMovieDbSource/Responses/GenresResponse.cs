using Domain;

namespace TheMovieDbSource.Responses;

public class GenresResponse
{
    public IEnumerable<Genre> Genres { get; set; }
}