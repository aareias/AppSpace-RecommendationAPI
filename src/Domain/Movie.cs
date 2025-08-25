namespace Domain;

public class Movie
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
    public string OriginalLanguage { get; set; }
    
    public bool Adult  { get; set; }
    
    public Genre[] Genres { get; set; }
}