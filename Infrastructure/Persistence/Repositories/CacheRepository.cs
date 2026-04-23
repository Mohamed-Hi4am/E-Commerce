using Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer connectionMultiplexer) : ICacheRepository
    {
        private readonly IDatabase database = connectionMultiplexer.GetDatabase();

        // Get cached data
        public async Task<string?> GetAsync(string key)
        {
            var value = await database.StringGetAsync(key);

            return value.IsNullOrEmpty ? default : value;
        }

        // Set cached data
        public async Task SetAsync(string key, object value, TimeSpan duration)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var serializedObject = JsonSerializer.Serialize(value, serializeOptions);

            await database.StringSetAsync(key, serializedObject, duration);
        }
    }
}