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
    public interface IGetHelpContentService
    {
        Task<HelpContentDto> Execute(string helpKey);
    }
    public class GetHelpContentService : IGetHelpContentService
    {
        private readonly IDataBaseContext _context;
        private readonly IMemoryCache _cache;

        public GetHelpContentService(IDataBaseContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<HelpContentDto> Execute(string helpKey)
        {
            // ۱. تلاش برای خواندن از Cache
            if (_cache.TryGetValue(helpKey, out HelpContentDto cachedContent))
            {
                return cachedContent;
            }

            // ۲. خواندن از دیتابیس در صورت نبود در کش
            var help = await _context.HelpContents
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.HelpKey == helpKey && x.IsActive);

            if (help == null)
            {
                return new HelpContentDto { Title = "خطا", Content = "راهنمایی برای این بخش تعریف نشده است." };
            }

            var result = new HelpContentDto
            {
                Title = help.Title,
                Content = help.Content
            };

            // ۳. ذخیره در Cache برای ۵ دقیقه
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(helpKey, result, cacheOptions);

            return result;
        }
    }
    public class HelpContentDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

}
