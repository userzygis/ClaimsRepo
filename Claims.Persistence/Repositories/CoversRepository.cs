using Azure.Core;
using Claims.Persistence.DbModels;
using Claims.Persistence.Interfaces;
using Microsoft.Azure.Cosmos;

namespace Claims.Persistence.Repositories
{
    public class CoversRepository: CosmosRepositoryBase<CoverModel>, ICoversRepository
    {
        public CoversRepository(Container container) : base(container)
        {
        }

        public async Task<IEnumerable<CoverModel>> GetCoversAsync()
        {
            return await base.GetItemsAsync();
        }

        public async Task<CoverModel> GetCoverAsync(string id)
        {
            return await base.GetItemAsync(id);
        }

        public async Task AddCoverAsync(CoverModel item)
        {
            await base.AddItemAsync(item);
        }

        public async Task DeleteCoverAsync(string id)
        {
            await base.DeleteItemAsync(id);
        }
    }
}
