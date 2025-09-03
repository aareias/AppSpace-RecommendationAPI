using SessionInformationDbSource.Entities.Abstractions;

namespace SessionInformationDbSource.Entities;

public class Room : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Size { get; set; }

    public int Seats { get; set; }
    
    public Cinema Cinema { get; set; }
}