namespace SessionsDB.Entities;

public class City
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int Population { get; set; }
    
    public IEnumerable<Cinema> Cinemas { get; set; }
}