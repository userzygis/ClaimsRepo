using Claims.Persistence.DbModels;
using Claims.Persistence.Interfaces;
using Microsoft.Azure.Cosmos;

namespace Claims.Persistence.Repositories
{
    public class ClaimsRepository: CosmosRepositoryBase<ClaimModel>, IClaimsRepository
    {
        public ClaimsRepository(Container container) : base(container)
        {
        }

        public async Task<IEnumerable<ClaimModel>> GetClaimsAsync()
        {
            return await base.GetItemsAsync();
        }

        public async Task<ClaimModel> GetClaimAsync(string id)
        {
            return await base.GetItemAsync(id);
        }

        public async Task AddClaimAsync(ClaimModel item)
        {
            await base.AddItemAsync(item);
        }

        public async Task DeleteClaimAsync(string id)
        {
            await base.DeleteItemAsync(id); 
        }
    }
}
