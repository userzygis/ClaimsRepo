namespace Claims.Domain.ActionModels.Responses.ClaimsResponses
{
    public class GetClaimsResponse : ResponseBase
    {
        public IEnumerable<Claim> Claims { get; set; }
    }
}
