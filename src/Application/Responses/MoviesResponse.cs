using Domain;

namespace Application.Responses;

public class MoviesResponse
{
    public IEnumerable<Domain.Movie> Movies { get; set; }
    
    public int TotalCount => Movies?.Count() ?? 0;
}