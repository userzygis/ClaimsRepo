namespace Claims.ActionModels.Responses.ClaimsResponses
{
    public class GetClaimsResponse: ResponseBase
    {
        public IEnumerable<Claim> Claims { get; set; }
    }
}
