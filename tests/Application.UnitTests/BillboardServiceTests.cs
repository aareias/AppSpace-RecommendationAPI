using Application.Configuration;
using Application.Requests;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SessionsDB.Repositories.Abstractions;
using TMDB.Client;
using TMDB.Client.Requests;
using TMDB.Client.Responses;
using Utils;
using SessionsDBMovie = SessionsDB.Entities.Movie;

namespace Application.UnitTests;

public class Tests
{
    static readonly string[] _genres = ["Genre1", "Genre2", "Genre3", "Genre4", "Genre5", "Genre6"];
    
    static readonly string _blockbusterGenres = string.Join(';', _genres.Take(3));
    
    static readonly int _maxSuccessfulMovies = 2;
    
    Mock<ITmdbClient> _clientMock = new();
    Mock<IMovieRepository> _movieRepositoryMock = new();

    BillboardService _service;
    
    [SetUp]
    public void Setup()
    {
        //Arrange
        var inMemorySettings = new Dictionary<string, string> {
            {ApplicationConstants.MaxSuccessfulMovies, _maxSuccessfulMovies.ToString()},
            {ApplicationConstants.BlockbusterGenres, _blockbusterGenres}
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        
        ApplicationSettings.SetConfiguration(configuration);
        
        _service = new BillboardService(_clientMock.Object, _movieRepositoryMock.Object);
    }
    
    IEnumerable<SessionsDBMovie> GetMovieSessionsTestData()
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData/MovieSessionsData.json"); 
        var reader = new StreamReader(filePath);
        var jsonStr = reader.ReadToEnd();
        var jsonObj = JObject.Parse(jsonStr);

        var testDataObj = jsonObj["movies"].ToString();
        var testData = JsonConvert.DeserializeObject<IEnumerable<SessionsDBMovie>>(testDataObj);

        return testData;
    }

    /// <summary>
    ///  This request should return 2 movies per week for 2 weeks.
    ///
    ///  Expected:
    ///   - Total Movies = 4
    ///   - Total Weeks = 2
    /// </summary>
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetBillboard_FilterBySuccessFalse_Success(bool filterBySuccessfulMovies)
    {
        // Arrange
        MockGenresResponse();
        
        MockSuccessfulMoviesResponse();
        
        MockMoviesResponse();
        
        var expectedWeeks = 2;
        var expectedMovies = 4;

        var startDate = new DateTime(2025, 1, 1); // Wednesday
        var endDate = new DateTime(2025, 1, 12); // Sunday

        List<(DateTime StartDate, DateTime EndDate)> expectedWeeksStartAndEndDates =
        [
            (new DateTime(2024, 12, 30), new DateTime(2025, 01, 05)),
            (new DateTime(2025, 01, 06), new DateTime(2025, 01, 12))
        ];
        
        var request = new GetIntelligentBillboardRequest()
        {
            BigRooms = 1,
            SmallRooms = 1,
            StartDate = startDate, // Wednesday
            EndDate = endDate, // Sunday
            FilterByMostSuccessful = filterBySuccessfulMovies
        };

        // Act
        var result = await _service.GetIntelligentBillboard(request, CancellationToken.None);

        // Assert
        Assert.That(result.TotalMovies, Is.EqualTo(expectedMovies));
        Assert.That(result.TotalWeeks, Is.EqualTo(expectedWeeks));
        Assert.That(result.WeekPlan.SelectMany(x => x.ScreenMovies).Select(y => y.Movie).Distinct().Count(), Is.EqualTo(expectedMovies));

        //  Validate week starts and ends are expected
        for (var i = 0; i < expectedWeeksStartAndEndDates.Count; i++)
        {
            var expectedWeek = expectedWeeksStartAndEndDates.ElementAt(i);
            var actualWeek = result.WeekPlan.ElementAt(i);

            Assert.That(actualWeek.ScreenMovies.Count(), Is.EqualTo(expectedMovies / expectedWeeks));
            Assert.That(actualWeek.StartDate, Is.EqualTo(expectedWeek.StartDate));
            Assert.That(actualWeek.EndDate, Is.EqualTo(expectedWeek.EndDate));
        }
    }

    void MockGenresResponse()
    {
        var genresList = new List<GenreResponse>();
        
        foreach (var genre in _genres)
        {
            genresList.Add(new GenreResponse
            {
                Id = genresList.Count + 1,
                Name = genre
            });
        }

        var genresResponse = new GenresResponse
        {
            Genres = genresList
        };
        
        _clientMock.Setup(x => x.GetMovieGenresAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(genresResponse);
    }

    void MockMoviesResponse()
    {
        var minorityMovies = new List<MovieResponse>
        {
            new()
            {
                Id = 1,
                OriginalTitle = "Movie1",
                ReleaseDate = new DateTime(2000, 1, 1),
                Overview = "Movie1",
                GenreIds = [4,5,6],
                Adult = false,
                Title = "Movie1",
                OriginalLanguage = "en"
            },
            new()
            {
                Id = 2,
                OriginalTitle = "Movie2",
                ReleaseDate = new DateTime(2000, 1, 1),
                Overview = "Movie2",
                GenreIds = [5,6],
                Adult = false,
                Title = "Movie2",
                OriginalLanguage = "en"
            }
        };
        
        var blockbusterMovies = new List<MovieResponse>
        {
            new()
            {
                Id = 3,
                OriginalTitle = "Movie3",
                ReleaseDate = new DateTime(2000, 1, 1),
                Overview = "Movie3",
                GenreIds = [1,2],
                Adult = false,
                Title = "Movie3",
                OriginalLanguage = "en"
            },
            new()
            {
                Id = 4,
                OriginalTitle = "Movie4",
                ReleaseDate = new DateTime(2000, 1, 1),
                Overview = "Movie4",
                GenreIds = [1],
                Adult = false,
                Title = "Movie4",
                OriginalLanguage = "en"
            }
        };

        _clientMock.Setup(x => x.GetMoviesAsync(It.Is<GetMoviesRequest>(x => x.WithoutGenres != null && x.WithoutGenres.Any()), It.IsAny<CancellationToken>()))
            .ReturnsAsync(minorityMovies);
        
        _clientMock.Setup(x => x.GetMoviesAsync(It.Is<GetMoviesRequest>(x => x.WithGenres != null && x.WithGenres.Any()), It.IsAny<CancellationToken>()))
            .ReturnsAsync(blockbusterMovies);
    }

    void MockSuccessfulMoviesResponse()
    {
        _movieRepositoryMock.Setup(x => x.GetMostSuccessfulMoviesAsync())
            .ReturnsAsync(GetMovieSessionsTestData);
    }
}