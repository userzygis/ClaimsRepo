using Claims.ActionModels.Requests.CoverRequests;
using Claims.Domain.ActionModels;

namespace Claims.Services.Interfaces
{
    public interface IPremiumCalcService
    {
        public Task<decimal> ComputePremiumAsync(IComputePremiumData data);
    }
}
