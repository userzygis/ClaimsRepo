using Claims.Domain.ActionModels;

namespace Claims.ActionModels.Requests.CoverRequests
{
    public class ComputePremiumRequest: IComputePremiumData
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public CoverType CoverType { get; set; }
    }

}
