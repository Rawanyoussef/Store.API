using StackExchange.Redis;
using System.Text.Json;

namespace Store.Service.CacheServices
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;
        public CacheService(IConnectionMultiplexer redis)
        {
            _database =redis.GetDatabase();

        }

        public async Task<string> GetCscheRespponseAsync(string key)
        {
          var cacheedResponse=await _database.StringGetAsync(key);
            if (cacheedResponse.IsNull)
                return null;
            return cacheedResponse.ToString();

        }

        public async Task SetCscheRespponseAsync(string Key, object response, TimeSpan timetolive)
        {
            if (response == null)
                return;
            var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var SerliaizedResponse = JsonSerializer.Serialize(response, option);
            await _database.StringSetAsync(Key, SerliaizedResponse, timetolive);
        }
    }
}
