namespace Claims.ActionModels.Responses.CoverResponses
{
    public class GetCoversResponse : ResponseBase
    {
        public IEnumerable<Cover> Covers { get; set; }
    }
}
