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
                var serviceRequest = new Application.Requests.GetIntelligentBillboardRequest()
                {
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
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
