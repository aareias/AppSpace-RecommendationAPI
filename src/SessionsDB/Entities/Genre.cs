using SessionsDB.Entities.Abstractions;

namespace SessionsDB.Entities;

public class Genre : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<Movie> Movies { get; set; }
}