using FitCore.Domain.Entities.TrainingProgram;
using FitCore.Persistence.Common;
using FitCore.Persistence.Contexts;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{
    public class TrainingDayTypeSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();

            var dayTypes = new List<TrainingDayType>
            {
                new TrainingDayType { Name = "بالاتنه" },
                new TrainingDayType { Name = "پایین‌تنه" },
                new TrainingDayType { Name = "فول‌بادی" },
                new TrainingDayType { Name = "هل (Push)" },
                new TrainingDayType { Name = "کشش (Pull)" },
                new TrainingDayType { Name = "پا (Leg)" },
                new TrainingDayType { Name = "سینه و سه‌سر" },
                new TrainingDayType { Name = "پشت و دوسر" },
                new TrainingDayType { Name = "شانه و زیربغل" },
                new TrainingDayType { Name = "کاردیو" },
                new TrainingDayType { Name = "استراحت" }
            };

            await context.SeedIfNotExists(
                dayTypes,
                dayType => x => x.Name == dayType.Name
            );
        }
    }
}