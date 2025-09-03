using Domain;

namespace Application.Responses;

public class GenresResponse
{
    public IEnumerable<Domain.Genre> Genres { get; set; }
    
    public int TotalCount => Genres?.Count() ?? 0;
}