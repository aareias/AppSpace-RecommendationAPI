using SessionInformationDbSource.Entities.Abstractions;

namespace SessionInformationDbSource.Entities;

public class Movie : IEntity
{
    public int Id { get; set; }
    
    public string OriginalTitle { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
    public string OriginalLanguage { get; set; }
    
    public bool Adult  { get; set; }
    
    public IEnumerable<Genre> Genres { get; set; }
}