using Application.Configuration;
using Application.Requests;
using Application.Responses;
using SessionsDB.Repositories.Abstractions;
using TMDB.Client;
using TMDB.Client.Requests;
using TMDB.Client.Responses;
using Utils;

namespace Application;

public class BillboardService(
    ITmdbClient tmdbClient,
    IMovieRepository movieRepository) : IBillboardService
{
    // List of genres typically associated with blockbuster movies
    static readonly string[] _standardBlockbusterGenres = ApplicationSettings.GetArrayValue(ApplicationConstants.BlockbusterGenres);

    static readonly int _maxSuccessfulMovies = ApplicationSettings.GetValue<int>(ApplicationConstants.MaxSuccessfulMovies);

    public async Task<BillboardResponse> GetIntelligentBillboard(
        GetIntelligentBillboardRequest request,
        CancellationToken cancellationToken)
    {
        var weeks = GetWeeks(request.StartDate, request.EndDate).ToList();

        var genresResponse = await tmdbClient.GetMovieGenresAsync(cancellationToken);

        List<int> blockbusterGenreIds;

        if (request.FilterByMostSuccessful)
        {
            var popularMovies = await movieRepository.GetMostSuccessfulMoviesAsync();

            blockbusterGenreIds = genresResponse.Genres.Where(g => popularMovies
                .Take(_maxSuccessfulMovies)
                .SelectMany(m => m.Genres.Select(g => g.Name))
                .Distinct().Contains(g.Name))
                .Select(g => g.Id)
                .ToList();
        }
        else
        {
            blockbusterGenreIds = genresResponse.Genres
                .Where(g => _standardBlockbusterGenres.Contains(g.Name))
                .Select(g => g.Id)
                .ToList();
        }

        var blockbusterGenreMovies = await tmdbClient.GetMoviesAsync(new GetMoviesRequest
        {
            WithGenres = blockbusterGenreIds,
            WantedMovies = request.BigRooms * weeks.Count
        }, cancellationToken);

        var minorityGenreMovies = await tmdbClient.GetMoviesAsync(new GetMoviesRequest
        {
            WithoutGenres = blockbusterGenreIds,
            WantedMovies = request.SmallRooms * weeks.Count
        }, cancellationToken);

        return GenerateBillBoard(
            blockbusterGenreMovies.ToList(), 
            minorityGenreMovies.ToList(), 
            genresResponse, 
            weeks, 
            request.BigRooms, 
            request.SmallRooms);
    }

    #region Private methods

    static BillboardResponse GenerateBillBoard(
        IEnumerable<MovieResponse> blockbusterMovies, 
        IEnumerable<MovieResponse> minorityMovies, 
        GenresResponse genres, 
        List<Week> weeks, 
        int bigRooms, 
        int smallRooms)
    {
        var billboard = new BillboardResponse();

        foreach (var week in weeks)
        {
            var bigRoomMovies = blockbusterMovies
                .Where(x => x.ReleaseDate <= week.StartDate)
                .Take(bigRooms)
                .ToList();
            
            blockbusterMovies = blockbusterMovies.Except(bigRoomMovies);
            
            var smallRoomMovies = minorityMovies
                .Where(x => x.ReleaseDate <= week.StartDate)
                .Take(smallRooms)
                .ToList();
            
            minorityMovies = minorityMovies.Except(smallRoomMovies);

            billboard.WeekPlan.Add(new WeekPlan
            {
                StartDate = week.StartDate,
                EndDate = week.EndDate,
                ScreenMovies = bigRoomMovies
                    .Select(m => new ScreenMovie
                    {
                        IsBigRoom = true,
                        Movie = new Movie
                        {
                            Id = m.Id,
                            Title = m.Title,
                            ReleaseDate = m.ReleaseDate,
                            OriginalLanguage = m.OriginalLanguage,
                            Adult = m.Adult,
                            Genres = m.GenreIds.Select(genre => new Genre { Id = genre, Name = genres.Genres.Single(g => g.Id == genre).Name }).ToArray()
                        }
                    })
                .Concat(smallRoomMovies
                    .Select(m => new ScreenMovie
                    {
                        IsBigRoom = false,
                        Movie = new Movie
                        {
                            Id = m.Id,
                            Title = m.Title,
                            ReleaseDate = m.ReleaseDate,
                            OriginalLanguage = m.OriginalLanguage,
                            Adult = m.Adult,
                            Genres = m.GenreIds.Select(gid => new Genre { Id = gid, Name = genres.Genres.Single(g => g.Id == gid).Name }).ToArray()
                        }
                    }))
            });
        }

        return billboard;
    }

    // Assuming monday is the first day of the week
    static IEnumerable<Week> GetWeeks(DateTime startDate, DateTime endDate)
    {
        startDate = startDate.DayOfWeek == DayOfWeek.Sunday ? startDate.AddDays(-6) : startDate.AddDays(DayOfWeek.Monday - startDate.DayOfWeek);
        
        var weekStart = startDate;
        var weekEnd = weekStart;

        while (weekEnd < endDate)
        {
            weekEnd = weekStart.AddDays(6);
            yield return new Week { StartDate = weekStart, EndDate = weekEnd };

            weekStart = weekStart.AddDays(7);
        };
    }

    #endregion
}