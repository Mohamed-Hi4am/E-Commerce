using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICacheRepository
    {
        // Cache the data/response
        public Task SetAsync(string key, object value, TimeSpan duration);

        // Get the cached data/response
        public Task<string?> GetAsync(string key);
    }
}