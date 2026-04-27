using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface ICacheService
    {
        public Task<string?> GetCachedValueAsync(string key);

        public Task SetCacheValueAsync(string key, object value, TimeSpan duration);
    }
}