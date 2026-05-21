using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public interface ILoginTokenStore
    {
        Task<string> CreateAsync(string mobile, TimeSpan ttl, CancellationToken cancellationToken = default);
        Task<string> GetMobileAsync(string token, CancellationToken cancellationToken = default);
        Task RemoveAsync(string token, CancellationToken cancellationToken = default);
    }
}
