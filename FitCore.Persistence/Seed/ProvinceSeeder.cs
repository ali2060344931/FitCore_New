using FitCore.Domain.Entities.Provinces;
using FitCore.Persistence.Contexts;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{
    /// <summary>
    /// استان ها
    /// </summary>
    public class ProvinceSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider
                .GetRequiredService<DataBaseContext>();

            if (context.Provinces.Any())
                return;

            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "AdminTemplate",
                "app-assets",
                "txt",
                "Provinces.txt");

            if (!File.Exists(path))
                return;

            var provinceNames = await File.ReadAllLinesAsync(path);

            List<Province> provinces = new();

            foreach (var item in provinceNames)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    provinces.Add(new Province
                    {
                        Name = item.Trim()
                    });
                }
            }

            await context.Provinces.AddRangeAsync(provinces);
            await context.SaveChangesAsync(default);
        }
    }
}