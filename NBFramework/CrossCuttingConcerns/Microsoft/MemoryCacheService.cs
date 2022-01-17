using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using NbFramework.Utilities.IoC;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NbFramework.CrossCuttingConcerns.Microsoft
{
    public class MemoryCacheService : ICacheService
    {
        IMemoryCache Cache;

        public MemoryCacheService()
        {
            Cache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
        }

        public void Add(string key, object data, int duration)
        {
            Cache.Set(key, data, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            return Cache.Get<T>(key);
        }

        public object Get(string key)
        {
            return Cache.Get(key);
        }

        public bool IsExist(string key)
        {
            return Cache.TryGetValue(key, out _);
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            PropertyInfo cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            dynamic cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(Cache) as dynamic;
            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            foreach (dynamic cacheItem in cacheEntriesCollection)
            {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }

            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            List<object> keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();

            foreach (var key in keysToRemove)
            {
                Cache.Remove(key);
            }
        }
    }
}
