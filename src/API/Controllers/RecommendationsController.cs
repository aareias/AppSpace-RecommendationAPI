using Application;
using Application.Requests;
using Application.Responses;
using Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class RecommendationsController(
    IRecommendationService recommendationService,
    ILogger<RecommendationsController> logger) : ControllerBase
{
    [HttpGet(Name = "GetIntelligentBillboard")]
    [Route("billboard/intelligent")]
    public async Task<Results<Ok<Billboard>, BadRequest<ProblemDetails>, NotFound<ProblemDetails>>> Get(
        [FromQuery] GetBillboardRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            return TypedResults.Ok(await recommendationService.GetIntelligentBillboard(request, cancellationToken));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting all genres");
            return TypedResults.BadRequest<ProblemDetails>(new()
            {
                Detail = ex.Message
            });
        }
    }
}