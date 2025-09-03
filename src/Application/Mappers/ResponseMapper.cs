using Domain;
using GenresResponse = Application.Responses.GenresResponse;
using MoviesResponse = Application.Responses.MoviesResponse;

namespace Application.Mappers;

public static class ResponseMapper
{
    public static GenresResponse ToAppResponse(this TheMovieDbSource.Responses.GenresResponse? response)
    {
        return new GenresResponse
        {
            Genres = response?.Genres ??  new List<Genre>(),
        };
    }
    
    public static MoviesResponse ToAppResponse(this TheMovieDbSource.Responses.DiscoverMoviesResponse response, TheMovieDbSource.Responses.GenresResponse genresResponse)
    {
        return new MoviesResponse
        {
            Movies = response.Results.Select(x => x.ToMovie(genresResponse)),
        };
    }

    static Movie ToMovie(this TheMovieDbSource.Responses.Movie movieResponse, TheMovieDbSource.Responses.GenresResponse genresResponse)
    {
        return new Movie
        {
            Id = movieResponse.Id,
            Title = movieResponse.Title,
            ReleaseDate = movieResponse.ReleaseDate,
            OriginalLanguage = movieResponse.OriginalLanguage,
            Adult = movieResponse.Adult,
            Genres = movieResponse.GenreIds.Select(x => genresResponse.Genres.Single(g => g.Id == x))
        };
    }
}