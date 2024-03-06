using Claims.Services.PremiumConditions;

namespace Claims.Services.Interfaces
{
    public interface IPremiumCondition
    {
        void SetNextCondition(IPremiumCondition nextCondition);

        decimal Execute(PremiumConditionParamsWrapper paramsWrapper);
    }
}
