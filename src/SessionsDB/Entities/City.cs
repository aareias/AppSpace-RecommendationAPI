using SessionsDB.Entities.Abstractions;

namespace SessionsDB.Entities;

public class City : IEntity
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int Population { get; set; }
    
    public IEnumerable<Cinema> Cinemas { get; set; }
}