using Claims.Domain.ActionModels;
using Claims.Services.Interfaces;
using Claims.Services.PremiumCalculators;

namespace Claims.Services.Impl
{
    public class PremiumCalcService : IPremiumCalcService
    {
        public async Task<decimal> ComputePremiumAsync(IComputePremiumData data)
        {
            var premiumCalculator = PremiumCalculatorFactory.GetPremiumCalculator(data.CoverType);
            return await Task.Run(() => premiumCalculator.Calculate(data.StartDate, data.EndDate, data.CoverType));
        }

        public int GetBaseDayRate(CoverType? coverType)
        {
            return PremiumCalculatorFactory.GetPremiumCalculator(coverType).BaseDayRate;
        }

        public decimal GetMultiplier(CoverType coverType)
        {
            return PremiumCalculatorFactory.GetPremiumCalculator(coverType).GetMultiplier(coverType);
        }
    }
}
