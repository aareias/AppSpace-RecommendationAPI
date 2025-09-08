using System.Net;
using Moq;
using Moq.Protected;
using TMDB.Client.Configurations;
using TMDB.Client.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Utils;

namespace TMDB.Client.UnitTests;

public class TmdbClientTests
{
    [SetUp]
    public void Setup()
    {
        //Arrange
        var inMemorySettings = new Dictionary<string, string> {
            {TMDBApplicationConstants.TMDBApiKey, "api_key"},
            {TMDBApplicationConstants.TMDBApiVersion, "1"},
            {TMDBApplicationConstants.TMDBBaseUrl, "http://fake.url"},
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        ApplicationSettings.SetConfiguration(configuration);
    }

    [Test]
    public async Task GetMovieGenresAsync_ReturnsGenresResponse()
    {
        // Arrange
        var expectedJson = "{\"genres\":[{\"id\":28,\"name\":\"Action\"}]}";

        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedJson),
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var loggerMock = new Mock<ILogger<TmdbClient>>();
        var tmdbClient = new TmdbClient(httpClient, loggerMock.Object);

        // Act
        var result = await tmdbClient.GetMovieGenresAsync(CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Genres.Count, Is.EqualTo(1));
        Assert.That(result.Genres.ToList()[0].Id, Is.EqualTo(28));
        Assert.That(result.Genres.ToList()[0].Name, Is.EqualTo("Action"));
    }

    [Test]
    public async Task GetMoviesAsync_ReturnsMovieResponses()
    {
        // Arrange
        var expectedJson = "{\"results\":[{\"id\":1,\"adult\":false,\"genre_ids\":[28,12],\"original_title\":\"Original Movie 1\",\"overview\":\"This is an overview.\",\"popularity\":7.5,\"release_date\":\"2023-01-01\",\"title\":\"Movie 1\",\"original_language\":\"en\",\"vote_average\":8.2,\"vote_count\":100}],\"total_pages\":1}";

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedJson),
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var loggerMock = new Mock<ILogger<TmdbClient>>();
        var tmdbClient = new TmdbClient(httpClient, loggerMock.Object);
        var request = new GetMoviesRequest { WantedMovies = 1 };

        // Act
        var result = await tmdbClient.GetMoviesAsync(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Id, Is.EqualTo(1));
        Assert.That(result.First().Title, Is.EqualTo("Movie 1"));
    }

    [Test]
    public void GetMovieGenresAsync_ThrowsException_WhenResponseIsNull()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{}"),
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var loggerMock = new Mock<ILogger<TmdbClient>>();
        var tmdbClient = new TmdbClient(httpClient, loggerMock.Object);

        // Act + Assert
        Assert.ThrowsAsync<InvalidDataException>(async () => await tmdbClient.GetMovieGenresAsync(CancellationToken.None));
    }

    [Test]
    public void GetMoviesAsync_ThrowsException_WhenResponseIsNull()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{}"),
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var loggerMock = new Mock<ILogger<TmdbClient>>();
        var tmdbClient = new TmdbClient(httpClient, loggerMock.Object);
        var request = new GetMoviesRequest { WantedMovies = 1 };

        // Act + Assert
        Assert.ThrowsAsync<InvalidDataException>(async () => await tmdbClient.GetMoviesAsync(request, CancellationToken.None));
    }

    [Test]
    public void GetMovieGenresAsync_ThrowsException_WhenStatusCodeIsNotOK()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var loggerMock = new Mock<ILogger<TmdbClient>>();
        var tmdbClient = new TmdbClient(httpClient, loggerMock.Object);

        // Act + Assert
        Assert.ThrowsAsync<HttpRequestException>(async () => await tmdbClient.GetMovieGenresAsync(CancellationToken.None));
    }

    [Test]
    public void GetMoviesAsync_ThrowsException_WhenStatusCodeIsNotOK()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var loggerMock = new Mock<ILogger<TmdbClient>>();
        var tmdbClient = new TmdbClient(httpClient, loggerMock.Object);
        var request = new GetMoviesRequest { WantedMovies = 1 };

        // Act + Assert
        Assert.ThrowsAsync<HttpRequestException>(async () => await tmdbClient.GetMoviesAsync(request, CancellationToken.None));
    }
}
