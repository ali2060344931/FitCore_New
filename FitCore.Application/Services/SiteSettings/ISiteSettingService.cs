using FitCore.Application.Interfaces.Contexts;

using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.SiteSettings
{
    public interface ISiteSettingService
    {
        SiteSettingDto Get();
    }


public class SiteSettingService : ISiteSettingService
    {
        private readonly IDataBaseContext _context;
        private readonly IMemoryCache _cache;

        public SiteSettingService(IDataBaseContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public SiteSettingDto Get()
        {
            var cacheKey = "SiteSettings";

            if (!_cache.TryGetValue(cacheKey, out SiteSettingDto settings))
            {
                var q = _context.Setings.FirstOrDefault(p => p.Id == 1);
                settings = new SiteSettingDto
                {
                    SiteName = q.TextFilde,
                    Email = q.Email,
                    Phone= q.Phone
                };

                _cache.Set(cacheKey, settings, TimeSpan.FromHours(12));
            }

            return settings;
        }
    }

    public class SiteSettingDto
    {
        public string SiteName { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
