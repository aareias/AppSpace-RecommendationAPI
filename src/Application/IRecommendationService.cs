using Application.Requests;
using Application.Responses;

namespace Application;

public interface IRecommendationService
{
    Task<Billboard> GetIntelligentBillboard(GetBillboardRequest request, CancellationToken cancellationToken);
}