namespace SessionsDB.Entities;

public class Genre
{
    public int Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<Movie> Movies { get; set; }
}