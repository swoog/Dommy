using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Dommy.Business.Tools
{
    public class Cache
    {
        private static MemoryCache cache = new MemoryCache("DommyCache");

        public static T Get<T>(string key, TimeSpan time, Func<T> get)
        {
            var obj = cache.Get(key);

            if (obj == null)
            {
                obj = get();
                cache.Add(key, obj, new DateTimeOffset(DateTime.Now.Add(time)));
            }

            return (T)obj;
        }
    }
}
