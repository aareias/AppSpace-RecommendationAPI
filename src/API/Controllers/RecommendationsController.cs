using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TheMovieDbSource;
using TheMovieDbSource.Responses;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class RecommendationsController(ITMDBClient itmdbClient, ILogger<RecommendationsController> logger) : ControllerBase
{
    [HttpGet(Name = "GetGenres")]
    [Route("genres/{media}")]
    public async Task<Results<Ok<GenresResponse>, BadRequest<ProblemDetails>, NotFound<ProblemDetails>>> Get([FromRoute] MediaType media, CancellationToken cancellationToken)
    {
        try
        {
            return TypedResults.Ok(await itmdbClient.GetGenresForType(media, cancellationToken));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting all genres");
            return TypedResults.BadRequest<ProblemDetails>(new (){
                Detail = ex.Message
            });
        }
    }
}