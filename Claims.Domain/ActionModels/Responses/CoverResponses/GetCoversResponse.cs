namespace Claims.Domain.ActionModels.Responses.CoverResponses
{
    public class GetCoversResponse : ResponseBase
    {
        public IEnumerable<Cover> Covers { get; set; }
    }
}
