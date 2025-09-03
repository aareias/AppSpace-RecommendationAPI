using SessionInformationDbSource.Entities.Abstractions;

namespace SessionInformationDbSource.Entities;

public class Session : IEntity
{
    public int Id { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public int SeatsSold { get; set; }
    
    public Room Room { get; set; }
    
    public Movie Movie { get; set; }
}