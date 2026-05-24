using FitCore.Domain.Entities.Provinces;
using FitCore.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{
    public class CitySeeder : ISeeder
    {
        /// <summary>
        /// شهر ها
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider
                .GetRequiredService<DataBaseContext>();

            if (context.Cities.Any())
                return;

            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "AdminTemplate",
                "app-assets",
                "txt",
                "City.txt");

            if (!File.Exists(path))
                return;

            var cityLines = await File.ReadAllLinesAsync(path);

            List<City> cities = new();

            foreach (var line in cityLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var data = line.Split(',');

                if (data.Length != 2)
                    continue;

                string provinceName = data[0].Trim();
                string cityName = data[1].Trim();

                var province = await context.Provinces
                    .FirstOrDefaultAsync(p => p.Name == provinceName);

                if (province != null)
                {
                    cities.Add(new City
                    {
                        Name = cityName,
                        ProvincesId = province.Id
                    });
                }
            }

            await context.Cities.AddRangeAsync(cities);
            await context.SaveChangesAsync();
        }
    }
}