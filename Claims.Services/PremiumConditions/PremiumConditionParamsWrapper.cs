namespace Claims.Services.PremiumConditions
{
    public class PremiumConditionParamsWrapper
    {
        public decimal PremiumPerDay { get; set; }
        public int InsuranceLength { get; set; }
        public decimal Discount1 { get; set; }
        public decimal Discount2 { get; set; }
    }
}
