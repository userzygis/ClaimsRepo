using Claims.Services.Interfaces;

namespace Claims.Services.PremiumConditions
{
    public abstract class PremiumConditionBase: IPremiumCondition
    {
        private IPremiumCondition _nextCondition;

        public void SetNextCondition(IPremiumCondition nextCondition)
        {
            _nextCondition = nextCondition;
        }

        public virtual decimal Execute(PremiumConditionParamsWrapper paramsWrapper)
        {
            if (_nextCondition != null)
            {
                return _nextCondition.Execute(paramsWrapper);
            }
            return 0m;
        }

        protected decimal GetNoDiscountTotalPremium(decimal premiumPerDay, int insuranceLength)
        {
            return insuranceLength * premiumPerDay;
        }

        protected decimal GetWithDiscountTotalPremium(decimal premiumPerDay, int insuranceLength, decimal discount)
        {
            return insuranceLength * (premiumPerDay - premiumPerDay * discount);
        }
    }
}
