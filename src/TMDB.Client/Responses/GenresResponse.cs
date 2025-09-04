using TMDB.Client.Responses;

namespace TMDB.Responses;

public class GenresResponse
{
    public IEnumerable<GenreResponse> Genres { get; set; } = new List<GenreResponse>();

    public int TotalCount => Genres?.Count() ?? 0;
}

