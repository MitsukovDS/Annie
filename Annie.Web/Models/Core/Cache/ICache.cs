using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Models.Core.Cache
{
    public interface ICache
    {
        //public bool TryGetValue<TItem>(CachedObject key, out TItem value);
        //public TItem Set<TItem>(CachedObject key, TItem value, TimeSpan offset);
        public bool TryGetValue<TItem>(Guid key, out TItem value);
        public TItem Set<TItem>(Guid key, TItem value, TimeSpan offset);
    }
}
