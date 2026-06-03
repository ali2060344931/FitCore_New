using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;
using FitCore.Persistence.Common;
using FitCore.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;
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


            /*
            var context = serviceProvider.GetRequiredService<DataBaseContext>();

            // اگر قبلاً داده‌ها وارد شده‌اند، تکراری وارد نکن
            //if (await context.Set<GoalType>().AnyAsync())
            //    return;

            //context.Set<GoalType>().AddRange(
            //      new GoalType { Name = "کاهش وزن" },
            //      new GoalType { Name = "افزایش عضله / عضله‌سازی" },
            //      new GoalType { Name = "حفظ وضعیت فعلی" },
            //      new GoalType { Name = "آمادگی برای مسابقه" }
            //  );



            var goals = new List<GoalType>
            {
            new GoalType { Name = "کاهش وزن" },
            new GoalType { Name = "افزایش عضله / عضله‌سازی" },
            new GoalType { Name = "حفظ وضعیت فعلی" },
            new GoalType { Name = "آمادگی برای مسابقه" },
            new GoalType { Name = "افزایش استقامت" },
            new GoalType { Name = "افزایش قدرت" }
            };


            foreach (var goal in goals)
            {
                if (!await context.Set<GoalType>().AnyAsync(x => x.Name == goal.Name))
                {
                    await context.Set<GoalType>().AddAsync(goal);
                }
            }
            await context.SaveChangesAsync(default);
            */
        }
    }


}
