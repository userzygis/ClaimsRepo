using Claims.Domain.ActionModels;
using Claims.Services.Interfaces;

namespace Claims.Services.PremiumCalculators
{
    public static class PremiumCalculatorFactory
    {
        public static IPremiumCalculator GetPremiumCalculator(CoverType? coverType)
        {
            if(coverType == CoverType.Yacht)
            {
                return new YachtPremiumCalculator();
            }
            return new DefaultPremiumCalculator();
        }
    }
}
