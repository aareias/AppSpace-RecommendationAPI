using SessionInformationDbSource.Entities.Abstractions;

namespace SessionInformationDbSource.Entities;

public class Cinema : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime OpenSince { get; set; }

    public IEnumerable<Room> Rooms { get; set; }
    
    public int CityId { get; set; }
}