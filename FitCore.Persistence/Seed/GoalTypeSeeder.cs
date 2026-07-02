using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;
using FitCore.Persistence.Common;
using FitCore.Persistence.Contexts;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{
    public class GoalTypeSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();
            var goalTypes = new List<GoalType>
            {
            new GoalType { Name = "کاهش وزن" },
            new GoalType { Name = "افزایش عضله / عضله‌سازی" },
            new GoalType { Name = "حفظ وضعیت فعلی" },
            new GoalType { Name = "آمادگی برای مسابقه" },
            new GoalType { Name = "افزایش استقامت" },
            new GoalType { Name = "افزایش قدرت" }
            };

            await context.SeedIfNotExists(
                goalTypes,
                goal => x => x.Name == goal.Name
            );

        }
    }


}
