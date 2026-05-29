using FitCore.Domain.Entities.NutritionProgram.Food;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;
using FitCore.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{
    public class GoalTypeSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();

            // اگر قبلاً داده‌ها وارد شده‌اند، تکراری وارد نکن
            if (await context.Set<GoalType>().AnyAsync())
                return;

            context.Set<GoalType>().AddRange(
                new GoalType {  Name = "کاهش وزن" },
                new GoalType {  Name = "افزایش عضله / عضله‌سازی" },
                new GoalType {  Name = "حفظ وضعیت فعلی" },
                new GoalType {  Name = "آمادگی برای مسابقه" }
            );

            await context.SaveChangesAsync(default);
        }
    }


}
