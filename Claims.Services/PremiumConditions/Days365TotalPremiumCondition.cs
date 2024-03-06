namespace Claims.Services.PremiumConditions
{
    public class Days365TotalPremiumCondition: Days150TotalPremiumCondition
    {
        protected const int insurance365DaysLength = 365;
        public override decimal Execute(PremiumConditionParamsWrapper paramsWrapper)
        {
            if (paramsWrapper.InsuranceLength <= insurance365DaysLength)
            {
                var paramsWrapper150days = new PremiumConditionParamsWrapper()
                {
                    InsuranceLength = insurance150DaysLength,
                    PremiumPerDay = paramsWrapper.PremiumPerDay,
                    Discount1 = paramsWrapper.Discount1                    
                };
                var totalPremium150daysWithDisc = base.Execute(paramsWrapper150days);
                var totalPremium365DaysWithDisc = GetWithDiscountTotalPremium(paramsWrapper.PremiumPerDay, paramsWrapper.InsuranceLength - insurance150DaysLength, paramsWrapper.Discount2); //0.01m
                return totalPremium150daysWithDisc + totalPremium365DaysWithDisc;
            }

            return base.Execute(paramsWrapper);
        }
    }
}
