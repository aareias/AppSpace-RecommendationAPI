using System.Globalization;
using Application.Configuration;
using Application.Requests;
using Application.Responses;
using SessionsDB.Repositories.Abstractions;
using TMDB;
using TMDB.Responses;
using Utils;

namespace Application;

public class BillboardService(
    ITmdbClient TmdbClient,
    IMovieRepository MovieRepository) : IBillboardService
{
    // List of genres typically associated with blockbuster movies
    static readonly string[] _standardBlockbusterGenres = ApplicationSettings.GetArrayValue(ApplicationConstants.BlockbusterGenres);

    static readonly int _maxSuccessfulMovies = ApplicationSettings.GetValue<int>(ApplicationConstants.MaxSuccessfulMovies);

    public async Task<BillboardResponse> GetIntelligentBillboard(GetBillboardRequest request, CancellationToken cancellationToken)
    {
        var weeks = GetWeeks(request.StartDate, request.EndDate);

        var genresResponse = await TmdbClient.GetMovieGenresAsync(cancellationToken);

        List<int> blockbusterGenreIds = new List<int>();

        if (request.FilterByMostSuccessful)
        {
            var popularMovies = await MovieRepository.GetMostSuccessfulMoviesAsync();

            blockbusterGenreIds = genresResponse.Genres.Where(g => popularMovies
                .Take(_maxSuccessfulMovies)
                .SelectMany(m => m.Genres.Select(g => g.Name)).Distinct().Contains(g.Name)).Select(g => g.Id).ToList();
        }
        else
        {
            blockbusterGenreIds = genresResponse.Genres.Where(g => _standardBlockbusterGenres.Contains(g.Name)).Select(g => g.Id).ToList();
        }

        var blockbusterGenreMovies = await TmdbClient.GetMoviesAsync(new GetMoviesRequest
        {
            WithGenres = blockbusterGenreIds,
            WantedMovies = request.BigRooms * weeks.Count(),
            ReleaseDateBeforeThan = request.StartDate
        }, cancellationToken);

        var minorityGenreMovies = await TmdbClient.GetMoviesAsync(new GetMoviesRequest
        {
            WithoutGenres = blockbusterGenreIds,
            WantedMovies = request.SmallRooms * weeks.Count(),
            ReleaseDateBeforeThan = request.StartDate
        }, cancellationToken);

        return GenerateBillBoard(blockbusterGenreMovies, minorityGenreMovies, genresResponse, weeks, request.BigRooms, request.SmallRooms);
    }

    #region Private methods

    static BillboardResponse GenerateBillBoard(IEnumerable<TMDB.Responses.Movie> blockbusterMovies, IEnumerable<TMDB.Responses.Movie> minorityMovies, TMDB.Responses.GenresResponse genres, IEnumerable<Week> weeks, int bigRooms, int smallRooms)
    {
        var billboard = new BillboardResponse();

        for (int i = 0; i < weeks.Count(); i++)
        {
            var week = weeks.ElementAt(i);
        
            var bigRoomMovies = blockbusterMovies.Skip(i * bigRooms).Take(bigRooms);
            var smallRoomMovies = minorityMovies.Skip(i * smallRooms).Take(smallRooms);

            billboard.WeekPlan.Add(new WeekPlan
            {
                StartDate = week.StartDate,
                EndDate = week.EndDate,
                ScreenMovies = bigRoomMovies.Select(m => new ScreenMovie
                {
                    IsBigRoom = true,
                    Movie = new Responses.Movie
                    {
                        Id = m.Id,
                        Title = m.Title,
                        ReleaseDate = m.ReleaseDate,
                        OriginalLanguage = m.OriginalLanguage,
                        Adult = m.Adult,
                        Genres = m.GenreIds.Select(genre => new Genre { Id = genre, Name = genres.Genres.Single(g => g.Id == genre).Name }).ToArray()
                    }
                })
                .Concat(smallRoomMovies.Select(m => new ScreenMovie
                {
                    IsBigRoom = false,
                    Movie = new Responses.Movie
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
        var initialWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
            startDate,
            CalendarWeekRule.FirstDay,
            DayOfWeek.Monday
        );

        var lastWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
            endDate,
            CalendarWeekRule.FirstDay,
            DayOfWeek.Monday
        );

        for (var week = initialWeek; week <= lastWeek; week++)
        {
            var weekStart = CultureInfo.CurrentCulture.Calendar.AddWeeks(startDate, week - initialWeek);
            var weekEnd = weekStart.AddDays(6);
            yield return new Week { StartDate = weekStart, EndDate = weekEnd };
        }
    }

    #endregion
}