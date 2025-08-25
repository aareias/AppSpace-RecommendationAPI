namespace Domain;

public class Session
{
    public int Id { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public int SeatsSold { get; set; }
    
    public Room Room { get; set; }
    
    public Movie Movie { get; set; }
}