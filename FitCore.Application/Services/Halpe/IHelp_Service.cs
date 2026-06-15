using FitCore.Application.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Halpe
{
    public interface IHelp_Service
    {
        Task<(string title, string content)> GetHelpAsync(string key);
    }

    public class HelpService : IHelp_Service
    {
        private readonly IDataBaseContext _context;
        private readonly IMemoryCache _cache;

        public HelpService(IDataBaseContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<(string title, string content)> GetHelpAsync(string key)
        {
            return await _cache.GetOrCreateAsync($"help_{key}", async entry => {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                var help = await _context.HelpContents.FirstOrDefaultAsync(x => x.HelpKey == key && x.IsActive);
                return help != null ? (help.Title, help.Content) : ("راهنما", "محتوایی یافت نشد.");
            });
        }
    }
}
