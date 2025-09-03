namespace Domain;

public class Cinema
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime OpenSince { get; set; }
    
    public IEnumerable<Room> Rooms { get; set; }
}