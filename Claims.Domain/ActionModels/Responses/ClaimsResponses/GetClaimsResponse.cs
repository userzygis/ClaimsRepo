namespace Claims.Domain.ActionModels.Responses.ClaimsResponses
{
    public class GetClaimsResponse
    {
        public IEnumerable<Claim> Claims { get; set; }
    }
}
