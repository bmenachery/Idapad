using System;
using System.Threading.Tasks;
using System.Text.Json;
using Infrastructure.Models;
using StackExchange.Redis;

namespace Infrastructure.DataAccess
{
    public class RedisDataAccess: IDisposable
    {

        public IDatabase _database ;
        private ConnectionMultiplexer _redisconnection;

        public RedisDataAccess(string connectionString)
        {
            _redisconnection = ConnectionMultiplexer.Connect(connectionString);
            _database = _redisconnection.GetDatabase();
        }


        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

       

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await _database.StringSetAsync(basket.Id,
                        JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            if (!created) return null;

            return await GetBasketAsync(basket.Id);
        }

        public void Dispose()
        {
            _redisconnection?.Dispose();
        }    
    }
}