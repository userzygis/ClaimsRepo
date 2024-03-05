using Claims.Domain.ActionModels;
using Claims.Services.Interfaces;

namespace Claims.Services.Impl
{
    public class PremiumCalcService: IPremiumCalcService
    {
        private const int baseDayRate = 1250;
        public async Task<decimal> ComputePremiumAsync(IComputePremiumData data)
        {
            //todo run each part async: if (i < 30), if (i < 180), if (i < 365)
            //todo create YachtPremiumCalculator, PassengerShipPremiumCalculator, TankerPremiumCalculator, DefaultPremiumCalculator and base with base logic
            return await Task.Run(() => ComputePremium(data.StartDate, data.EndDate, data.CoverType));
        }

        public int BaseDayRate { get { return baseDayRate; } }

        public decimal GetMultiplier(CoverType coverType)
        {
            var multiplier = 1.3m;
            if (coverType == CoverType.Yacht)
            {
                multiplier = 1.1m;
            }

            if (coverType == CoverType.PassengerShip)
            {
                multiplier = 1.2m;
            }

            if (coverType == CoverType.Tanker)
            {
                multiplier = 1.5m;
            }
            return multiplier;
        }

        private decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
        {
            var multiplier = GetMultiplier(coverType);
            var premiumPerDay = baseDayRate * multiplier;
            var insuranceLength = endDate.DayNumber - startDate.DayNumber;
            var totalPremium = 0m;

            //todo refact - this iteration not needed:
            for (var i = 0; i < insuranceLength; i++)
            {
                if (i < 30) totalPremium += premiumPerDay;
                else if (i < 150 && coverType == CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.05m;
                else if (i < 150) totalPremium += premiumPerDay - premiumPerDay * 0.02m;
                else if (i < 365 && coverType == CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.03m;
                else if (i < 365) totalPremium += premiumPerDay - premiumPerDay * 0.01m;
            }

            return totalPremium;
        }
    }
}
