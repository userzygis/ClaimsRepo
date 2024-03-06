using Claims.Domain.ActionModels;
using Claims.Services.Interfaces;
using Claims.Services.PremiumConditions;

namespace Claims.Services.PremiumCalculators
{
    public class DefaultPremiumCalculator: IPremiumCalculator
    {
        public virtual int BaseDayRate { get { return 1250; } }

        public decimal Calculate(DateOnly startDate, DateOnly endDate, CoverType coverType)
        {
            var multiplier = GetMultiplier(coverType);
            var premiumPerDay = BaseDayRate * multiplier;
            var insuranceLength = endDate.DayNumber - startDate.DayNumber;            
            return CalculateCore(premiumPerDay, insuranceLength, CreateConditionsChain());
        }

        protected virtual decimal CalculateCore(decimal premiumPerDay, int insuranceLength, IPremiumCondition mainCondition)
        {
            return mainCondition.Execute(new PremiumConditionParamsWrapper()
            {
                PremiumPerDay = premiumPerDay,
                InsuranceLength = insuranceLength,
                Discount1 = 0.02m,
                Discount2 = 0.01m,
            });
        }

        public virtual decimal GetMultiplier(CoverType coverType)
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

        protected virtual IPremiumCondition CreateConditionsChain()
        {
            IPremiumCondition cond1 = new First30DaysNoDiscountTotalPremiumCondition();
            IPremiumCondition cond2 = new Days150TotalPremiumCondition();
            IPremiumCondition cond3 = new Days365TotalPremiumCondition();
            cond1.SetNextCondition(cond2);
            cond2.SetNextCondition(cond3);
            return cond1;
        }
    }
}
