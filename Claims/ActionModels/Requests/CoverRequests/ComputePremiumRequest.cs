namespace Claims.ActionModels.Requests.CoverRequests
{
    public class ComputePremiumRequest
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public CoverType CoverType { get; set; }
    }

}
