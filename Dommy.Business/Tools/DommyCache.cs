//-----------------------------------------------------------------------
// <copyright file="DommyCache.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Tools
{
    using System;
    using System.Runtime.Caching;

    public static class DommyCache
    {
        private static MemoryCache cache = new MemoryCache("DommyCache");

        public static T Get<T>(string key, TimeSpan time, Func<T> getValue)
        {
            var obj = cache.Get(key);

            if (obj == null)
            {
                obj = getValue();
                cache.Add(key, obj, new DateTimeOffset(DateTime.Now.Add(time)));
            }

            if (obj == null)
            {
                return default(T);
            }

            return (T)obj;
        }
    }
}
