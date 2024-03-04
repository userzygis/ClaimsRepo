using Claims.Persistence.DbModels;
using Microsoft.Azure.Cosmos;

namespace Claims.Persistence.Repositories
{
    public abstract class CosmosRepositoryBase<T> where T : CosmoModel
    {
        protected readonly Container _container;
        protected CosmosRepositoryBase(Container container) 
        { 
            _container = container;
        }

        protected async Task<IEnumerable<T>> GetItemsAsync()
        {
            var query = _container.GetItemQueryIterator<T>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }
            return results;
        }

        protected async Task<T> GetItemAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<T>(id, new(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        protected async Task AddItemAsync(T item)
        {
            item.Id = Guid.NewGuid().ToString();
            await _container.CreateItemAsync(item, new PartitionKey(item.Id));
        }

        protected async Task DeleteItemAsync(string id)
        {
            await _container.DeleteItemAsync<T>(id, new PartitionKey(id));
        }
    }
}
