namespace Claims.Services.PremiumConditions
{
    public class Days150TotalPremiumCondition: First30DaysNoDiscountTotalPremiumCondition
    {
        protected const int insurance150DaysLength = 150;
        public override decimal Execute(PremiumConditionParamsWrapper paramsWrapper)
        {
            if (paramsWrapper.InsuranceLength <= insurance150DaysLength)
            {
                var paramsWrapper30days = new PremiumConditionParamsWrapper()
                {
                    InsuranceLength = insurance30DaysLength,
                    PremiumPerDay = paramsWrapper.PremiumPerDay,
                };
                var totalPremiumFirst30days = base.Execute(paramsWrapper30days);
                var totalPremium120daysWithDisc = GetWithDiscountTotalPremium(paramsWrapper.PremiumPerDay, paramsWrapper.InsuranceLength - insurance30DaysLength, paramsWrapper.Discount1); //discount  0.02m
                return totalPremiumFirst30days + totalPremium120daysWithDisc;
            }

            return base.Execute(paramsWrapper);
        }
    }
}
