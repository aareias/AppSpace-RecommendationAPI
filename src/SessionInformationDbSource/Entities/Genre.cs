using SessionInformationDbSource.Entities.Abstractions;

namespace SessionInformationDbSource.Entities;

public class Genre : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<Movie> Movies { get; set; }
}