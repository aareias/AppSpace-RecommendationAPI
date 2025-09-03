using Microsoft.EntityFrameworkCore;
using SessionInformationDbSource.Entities;
using Shared;

namespace SessionInformationDbSource.Configuration;

public class SessionContext(DbContextOptions<SessionContext> options) : DbContext(options)
{
    static string ConnectionString => ApplicationSettings.GetValue(SessionInfoConstants.ConnectionString);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cinema>()
            .HasOne<City>()
            .WithMany(c => c.Cinemas)
            .HasForeignKey(c => c.CityId);
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
            .WithMany();
    }
}