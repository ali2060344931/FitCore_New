using FitCore.Domain.Entities.Members;
using FitCore.Persistence.Common;
using FitCore.Persistence.Contexts;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{
    public class ActivityLevelSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();
            var goalTypes = new List<ActivityLevel>
            {
            new ActivityLevel { Name = "بی‌تحرک (کم‌تحرک)" },
            new ActivityLevel { Name = "فعالیت سبک" },
            new ActivityLevel { Name = "فعالیت متوسط" },
            new ActivityLevel { Name = "فعالیت سنگین" },
            new ActivityLevel { Name = "ورزشکار حرفه‌ای" },
            };

            await context.SeedIfNotExists(
                goalTypes,
                goal => x => x.Name == goal.Name
            );
        }

    }
}
