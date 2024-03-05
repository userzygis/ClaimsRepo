using Claims.Domain.ActionModels;

namespace Claims.Services.Interfaces
{
    public interface IPremiumCalcService
    {
        Task<decimal> ComputePremiumAsync(IComputePremiumData data);
        int BaseDayRate { get; }
        decimal GetMultiplier(CoverType coverType);
    }
}
