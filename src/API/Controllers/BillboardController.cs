using API.Model;
using Application;
using Application.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillboardController(
        IBillboardService recommendationService,
        ILogger<BillboardController> logger) : ControllerBase
    {
        [HttpGet(Name = "GetIntelligentBillboard")]
        [Route("intelligent")]
        public async Task<Results<Ok<BillboardResponse>, BadRequest<ProblemDetails>, NotFound<ProblemDetails>>> GetIntelligent(
            [FromQuery] GetIntelligentBillboardRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (request.Since >= request.Until)
                {
                    return TypedResults.BadRequest<ProblemDetails>(new()
                    {
                        Detail = "End date must be after start date."
                    });
                }

                if (request.BigRooms < 1 && request.SmallRooms < 1)
                {
                    return TypedResults.BadRequest<ProblemDetails>(new()
                    {
                        Detail = "There must be at least one big room or one small room."
                    });
                }

                var serviceRequest = new Application.Requests.GetIntelligentBillboardRequest()
                {
                    StartDate = request.Since,
                    EndDate = request.Until,
                    BigRooms = request.BigRooms,
                    SmallRooms = request.SmallRooms,
                    FilterByMostSuccessful = request.FilterByMostSuccessful,
                };
                
                return TypedResults.Ok(await recommendationService.GetIntelligentBillboard(serviceRequest, cancellationToken));
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
