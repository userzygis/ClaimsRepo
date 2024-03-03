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
            //var query = _container.GetItemQueryIterator<CoverModel>(new QueryDefinition("SELECT * FROM c"));
            //var results = new List<CoverModel>();
            //while (query.HasMoreResults)
            //{
            //    var response = await query.ReadNextAsync();

            //    results.AddRange(response.ToList());
            //}
            //return results;
            return await base.GetItemsAsync();
        }

        public async Task<CoverModel> GetCoverAsync(string id)
        {
            //try
            //{
            //    var response = await _container.ReadItemAsync<CoverModel>(id, new(id));
            //    return response.Resource;
            //}
            //catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            //{
            //    return null;
            //}
            return await base.GetItemAsync(id);
        }

        public async Task AddCoverAsync(CoverModel item)
        {
            await base.AddItemAsync(item);
            //await _container.CreateItemAsync(item, new PartitionKey(item.Id));
        }

        public async Task DeleteCoverAsync(string id)
        {
            await base.DeleteItemAsync(id);
            //await _container.DeleteItemAsync<CoverModel>(id, new PartitionKey(id));
        }
    }
}
