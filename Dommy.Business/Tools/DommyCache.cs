//-----------------------------------------------------------------------
// <copyright file="DommyCache.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Tools
{
    using System;
    using System.Runtime.Caching;

    /// <summary>
    /// Dommy cache.
    /// </summary>
    public static class DommyCache
    {
        /// <summary>
        /// Memory cache used.
        /// </summary>
        private static readonly MemoryCache Cache = new MemoryCache("DommyCache");

        /// <summary>
        /// Get a key from the cache.
        /// </summary>
        /// <typeparam name="T">Type of the element.</typeparam>
        /// <param name="key">Key of the element.</param>
        /// <param name="time">Expiration time.</param>
        /// <param name="getValue">Function used for get the value on expiration.</param>
        /// <returns>Value cached.</returns>
        public static T Get<T>(string key, TimeSpan time, Func<T> getValue)
        {
            var obj = Cache.Get(key);

            if (obj == null)
            {
                obj = getValue();
                Cache.Add(key, obj, new DateTimeOffset(DateTime.Now.Add(time)));
            }

            if (obj == null)
            {
                return default(T);
            }

            return (T)obj;
        }
    }
}
