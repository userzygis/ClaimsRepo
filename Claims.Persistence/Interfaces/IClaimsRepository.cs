using Claims.Persistence.DbModels;

namespace Claims.Persistence.Interfaces
{
    public interface IClaimsRepository
    {
        Task<IEnumerable<ClaimModel>> GetClaimsAsync();
        Task<ClaimModel> GetClaimAsync(string id);
        Task AddClaimAsync(ClaimModel item);
        Task DeleteClaimAsync(string id);
    }
}
