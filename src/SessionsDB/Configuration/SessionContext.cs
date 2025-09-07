using Microsoft.EntityFrameworkCore;
using SessionsDB.Entities;

namespace SessionsDB.Configuration;

public class SessionContext(DbContextOptions<SessionContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cinema>()
            .HasOne(ci => ci.City)
            .WithMany(c => c.Cinemas);
        modelBuilder.Entity<City>();
        modelBuilder.Entity<Genre>()
            .HasMany(g => g.Movies)
            .WithMany(m => m.Genres)
            .UsingEntity<Dictionary<string, object>>(
                "MovieGenre",
                j => j.HasOne<Movie>().WithMany().HasForeignKey("MovieId"),
                j => j.HasOne<Genre>().WithMany().HasForeignKey("GenreId")
            );
        modelBuilder.Entity<Movie>();
        modelBuilder.Entity<Room>()
            .HasOne(c => c.Cinema)
            .WithMany(c => c.Rooms);
        modelBuilder.Entity<Session>()
            .HasOne(s => s.Room)
            .WithMany();
        modelBuilder.Entity<Session>()
            .HasOne(s => s.Movie)
            .WithMany(m => m.Sessions);
    }
}