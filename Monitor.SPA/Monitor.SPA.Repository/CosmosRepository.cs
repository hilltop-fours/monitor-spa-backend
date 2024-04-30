using Microsoft.Azure.Cosmos;
using Monitor.SPA.Models.Interface;
using Monitor.SPA.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monitor.SPA.Repository
{
    public class CosmosRepository<T> : IGenericRepository<T> where T : IBaseEntity
    {
        private readonly Container _container;

        public CosmosRepository(Container container) => _container = container;

        public async Task<T> CreateAsync(T entity)
        {
            return await _container.CreateItemAsync(entity, new PartitionKey(entity.SubscriptionId));
        }

        public async Task<T> ReadAsync(string id, long subscriptionId)
        {
            return await _container.ReadItemAsync<T>(id, new PartitionKey(subscriptionId));
        }

        public async Task<IEnumerable<T>> ReadListAsync(long subscriptionId)
        {
            var list = new List<T>();
            var requestOptions = new QueryRequestOptions() { PartitionKey = new PartitionKey(subscriptionId) };
            using var resultSet = _container.GetItemQueryIterator<T>(requestOptions: requestOptions);
            while (resultSet.HasMoreResults)
            {
                var response = await resultSet.ReadNextAsync();
                list.AddRange(response);
            }
            return list;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            return await _container.ReplaceItemAsync(entity, entity.Id, new PartitionKey(entity.SubscriptionId));
        }

        public async Task DeleteAsync(T entity)
        {
            await _container.DeleteItemAsync<T>(entity.Id, new PartitionKey(entity.SubscriptionId));
        }
    }
}
