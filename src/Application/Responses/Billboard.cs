namespace Application.Responses;

public class Billboard
{
    public List<WeekPlan> WeekPlan { get; set; } = new List<WeekPlan>();
}

public class ScreenMovie
{
    public bool IsBigRoom { get; set; }
    
    public Movie Movie { get; set; }
}

public class Movie
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
    public string OriginalLanguage { get; set; }
    
    public bool Adult  { get; set; }
    
    public Genre[] Genres { get; set; }
}

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class WeekPlan
{
    public IEnumerable<ScreenMovie> ScreenMovies { get; set; }

    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}