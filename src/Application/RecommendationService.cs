using System.Data.Common;
using System.Globalization;
using System.Reflection.Metadata;
using Application.Mappers;
using Application.Requests;
using Application.Responses;
using Domain;
using SessionInformationDbSource.Repositories;
using TheMovieDbSource;
using MoviesResponse = Application.Responses.MoviesResponse;

namespace Application;

public class RecommendationService(
    ITmdbClient TmdbClient,
    ICityRepository CityRepository,
    ISessionRepository SessionRepository,
    IGenreRepository GenreRepository) : IRecommendationService
{
    // List of genres typically associated with blockbuster movies
    static readonly string[] standardBlockbusterGenres = ["Action", "Adventure", "Animation", "Fantasy", "Science Fiction", "Family"];

    public async Task<Billboard> GetIntelligentBillboard(GetBillboardRequest request, CancellationToken cancellationToken)
    {
        var weeks = GetWeeks(request.StartDate, request.EndDate);

        var genresResponse = await TmdbClient.GetMovieGenresAsync(cancellationToken);

        var blockbusterGenreIds = genresResponse.Genres.Where(g => standardBlockbusterGenres.Contains(g.Name, StringComparer.OrdinalIgnoreCase)).Select(g => g.Id);

        if (request.FilterByMostSuccessful)
        {
            // TODO
        }

        var blockbusterGenreMovies = await TmdbClient.GetMoviesAsync(new TheMovieDbSource.Responses.GetMoviesRequest
        {
            WithGenres = blockbusterGenreIds,
            WantedMovies = request.BigRooms * weeks.Count(),
            ReleaseDateBeforeThan = request.StartDate
        }, cancellationToken);

        var minorityGenreMovies = await TmdbClient.GetMoviesAsync(new TheMovieDbSource.Responses.GetMoviesRequest
        {
            WithoutGenres = blockbusterGenreIds,
            WantedMovies = request.SmallRooms * weeks.Count(),
            ReleaseDateBeforeThan = request.StartDate
        }, cancellationToken);

        return GenerateBillBoard(blockbusterGenreMovies, minorityGenreMovies, genresResponse, weeks, request.BigRooms, request.SmallRooms);
    }

    #region Private methods

    static Billboard GenerateBillBoard(IEnumerable<TheMovieDbSource.Responses.Movie> blockbusterMovies, IEnumerable<TheMovieDbSource.Responses.Movie> minorityMovies, TheMovieDbSource.Responses.GenresResponse genres, IEnumerable<Week> weeks, int bigRooms, int smallRooms)
    {
        var billboard = new Billboard();

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
                        Genres = m.GenreIds.Select(genre => new Responses.Genre { Id = genre, Name = genres.Genres.Single(g => g.Id == genre).Name }).ToArray()
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
                        Genres = m.GenreIds.Select(gid => new Responses.Genre { Id = gid, Name = genres.Genres.Single(g => g.Id == gid).Name }).ToArray()
                    }
                }))
            });
        }

        return billboard;
    }

    async Task<IEnumerable<Domain.Genre>> GetSuccessfulGenresAsync()
    {
        var response = await GenreRepository.GetGenresByPopularityAsync();
        return response.Select(g => new Domain.Genre
        {
            Id = g.Id,
            Name = g.Name
        });
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