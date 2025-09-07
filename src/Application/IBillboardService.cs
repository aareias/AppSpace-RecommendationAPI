using Application.Requests;
using Application.Responses;

namespace Application;

public interface IBillboardService
{
    Task<BillboardResponse> GetIntelligentBillboard(GetIntelligentBillboardRequest request, CancellationToken cancellationToken);
}