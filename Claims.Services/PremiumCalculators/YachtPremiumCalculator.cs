using Claims.Services.Interfaces;
using Claims.Services.PremiumConditions;

namespace Claims.Services.PremiumCalculators
{
    public class YachtPremiumCalculator: DefaultPremiumCalculator
    {
        protected override decimal CalculateCore(decimal premiumPerDay, int insuranceLength, IPremiumCondition mainCondition)
        {            
            return mainCondition.Execute(new PremiumConditionParamsWrapper()
            {
                PremiumPerDay = premiumPerDay,
                InsuranceLength = insuranceLength,
                Discount1 = 0.05m,
                Discount2 = 0.03m,
            });
        }
    }
}
