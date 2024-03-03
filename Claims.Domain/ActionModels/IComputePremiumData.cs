namespace Claims.Domain.ActionModels
{
    public interface IComputePremiumData
    {
        DateOnly StartDate { get; }
        DateOnly EndDate { get; }
        CoverType CoverType { get; }
    }
}
