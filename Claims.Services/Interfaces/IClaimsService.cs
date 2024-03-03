using Claims.Domain.ActionModels;

namespace Claims.Services.Interfaces
{
    public interface IClaimsService
    {
        Task<IEnumerable<Claim>> GetClaimsAsync();
        Task<Claim> GetClaimAsync(string id);

        Task<string> AddClaimAsync(Claim item);

        Task DeleteClaimAsync(string id);
    }
}
