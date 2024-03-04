namespace Claims.Domain.ActionModels.Requests.CoverRequests
{
    public class DeleteCoverRequest
    {
        public string Id { get; set; }
        public string? Reason { get; set; }
    }
}
