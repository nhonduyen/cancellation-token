using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using cancel.Models;
//https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-2.2
namespace cancel.Controllers
{
    public class CacheController : Controller
    {
        private readonly IMemoryCache _cache;

        public CacheController(IMemoryCache cache)
        {
            _cache = cache;
        }
        public IActionResult Index()
        {
          
            return View("Cache");
        }

        public IActionResult CacheTryGetValueSet()
        {
            DateTime cacheEntry;

            if (!_cache.TryGetValue(CacheKeys.Entry, out cacheEntry))
            {
                // key not in cache
                cacheEntry = DateTime.Now;
                // set cache option
                var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(10)); // time cache idle before removed

                // save data in cache
                _cache.Set(CacheKeys.Entry, cacheEntry, cacheOptions);
            }
            return View("Cache", cacheEntry);
        }

         public async Task<IActionResult> CacheGetOrCreateAsync()
        {
            var cacheEntry = await 
            _cache.GetOrCreateAsync(CacheKeys.Entry, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                return Task.FromResult(DateTime.Now);
            });

            return View("Cache", cacheEntry);
        }

         public IActionResult CacheGet()
        {
            var cacheEntry = _cache.Get<DateTime?>(CacheKeys.Entry);

            return View("Cache", cacheEntry);
        }
    }
}