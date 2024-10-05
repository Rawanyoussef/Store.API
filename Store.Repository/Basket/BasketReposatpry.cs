using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Store.Repository.Basket.Models;
using System.Text.Json;

namespace Store.Repository.Basket
{
    public class BasketReposatpry : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketReposatpry(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();

        }
        public async Task<bool> DeleteBasketAsync(string basketId)
            => await _database.KeyDeleteAsync(basketId);
   

        public async Task<CustomerBasket> GEtBasketAsync(string basketId)
        {
            var basket =await _database.StringGetAsync(basketId);

            return basket.IsNullOrEmpty ? null :JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var isCreated = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            if (!isCreated)
            {
                return null;
            }
            return  await GEtBasketAsync(basket.Id);
        }
    }
}
