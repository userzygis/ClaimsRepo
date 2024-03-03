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
            //var query = _container.GetItemQueryIterator<ClaimModel>(new QueryDefinition("SELECT * FROM c"));
            //var results = new List<ClaimModel>();
            //while (query.HasMoreResults)
            //{
            //    var response = await query.ReadNextAsync();

            //    results.AddRange(response.ToList());
            //}
            //return results;
            return await base.GetItemsAsync();
        }

        public async Task<ClaimModel> GetClaimAsync(string id)
        {
            //try
            //{
            //    var response = await _container.ReadItemAsync<ClaimModel>(id, new PartitionKey(id));
            //    return response.Resource;
            //}
            //catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            //{
            //    return null;
            //}

            return await base.GetItemAsync(id);
        }

        public async Task AddClaimAsync(ClaimModel item)
        {
            await base.AddItemAsync(item);
            //await _container.CreateItemAsync(item, new PartitionKey(item.Id));
        }

        public async Task DeleteClaimAsync(string id)
        {
            await base.DeleteItemAsync(id); 
            //await _container.DeleteItemAsync<ClaimModel>(id, new PartitionKey(id));
        }
    }
}
