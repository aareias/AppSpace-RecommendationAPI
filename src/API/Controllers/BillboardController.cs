using Application;
using Application.Requests;
using Application.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillboardController(
        IBillboardService RecommendationService,
        ILogger<BillboardController> logger) : ControllerBase
    {
        [HttpGet(Name = "GetIntelligentBillboard")]
        [Route("intelligent")]
        public async Task<Results<Ok<BillboardResponse>, BadRequest<ProblemDetails>, NotFound<ProblemDetails>>> Get(
            [FromQuery] GetBillboardRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                return TypedResults.Ok(await RecommendationService.GetIntelligentBillboard(request, cancellationToken));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while building intelligent billboard.");
                return TypedResults.BadRequest<ProblemDetails>(new()
                {
                    Detail = ex.Message
                });
            }
        }
    }
}
