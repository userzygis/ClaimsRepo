using Claims.Domain.ActionModels;

namespace Claims.Services.Interfaces
{
    public interface IPremiumCalculator
    {
        decimal Calculate(DateOnly startDate, DateOnly endDate, CoverType coverType);
        decimal GetMultiplier(CoverType coverType);
        int BaseDayRate { get; }
    }
}
