namespace Claims.Services.PremiumConditions
{
    public class First30DaysNoDiscountTotalPremiumCondition: PremiumConditionBase
    {
        protected const int insurance30DaysLength = 30;
        public override decimal Execute(PremiumConditionParamsWrapper paramsWrapper)
        {
            if (paramsWrapper.InsuranceLength <= insurance30DaysLength)
            {
                return GetNoDiscountTotalPremium(paramsWrapper.PremiumPerDay, paramsWrapper.InsuranceLength);
            }

            return base.Execute(paramsWrapper);
        }
    }
}
