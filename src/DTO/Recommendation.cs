using Domain;

namespace DTO;

public abstract class Recommendation
{
    public string Title { get; set; }
    
    public string Overview { get; set; }
    
    public Genre[] Genres { get; set; }
    
    public string Language { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
    public string Website { get; set; }
    
    public string[] Keywords { get; set; }
}