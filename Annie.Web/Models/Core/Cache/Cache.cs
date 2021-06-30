using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Models.Core.Cache
{
    public class Cache : ICache
    {
        private readonly IMemoryCache _memoryCache;

        public Cache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public TItem Set<TItem>(Guid key, TItem value, TimeSpan offset)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(offset);
            return _memoryCache.Set(key, value, cacheEntryOptions);
        }

        public bool TryGetValue<TItem>(Guid key, out TItem value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }

        //public TItem Set<TItem>(CachedObject key, TItem value, TimeSpan offset)
        //{
        //    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(offset);
        //    return _memoryCache.Set(key, value, cacheEntryOptions);
        //}

        //public bool TryGetValue<TItem>(CachedObject key, out TItem value)
        //{
        //    return _memoryCache.TryGetValue(key, out value);
        //}
    }
}
