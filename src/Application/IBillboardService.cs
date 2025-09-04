using Application.Requests;
using Application.Responses;

namespace Application;

public interface IBillboardService
{
    Task<BillboardResponse> GetIntelligentBillboard(GetBillboardRequest request, CancellationToken cancellationToken);
}