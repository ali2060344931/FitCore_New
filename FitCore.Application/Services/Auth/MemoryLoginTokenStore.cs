using Microsoft.Extensions.Caching.Memory;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public class MemoryLoginTokenStore : ILoginTokenStore
    {
        private readonly IMemoryCache _cache;

        public MemoryLoginTokenStore(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<string> CreateAsync(string mobile, TimeSpan ttl, CancellationToken cancellationToken = default)
        {
            var token = Guid.NewGuid().ToString("N");
            _cache.Set(GetKey(token), mobile, ttl);
            return Task.FromResult(token);
        }

        public Task<string> GetMobileAsync(string token, CancellationToken cancellationToken = default)
        {
            _cache.TryGetValue(GetKey(token), out string mobile);
            return Task.FromResult(mobile);
        }

        public Task RemoveAsync(string token, CancellationToken cancellationToken = default)
        {
            _cache.Remove(GetKey(token));
            return Task.CompletedTask;
        }

        private static string GetKey(string token) => $"login-token:{token}";
    }
}
