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

    public class NutritionProgramTypeSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();

            // اگر قبلاً داده‌ها وارد شده‌اند، تکراری وارد نکن
            if (await context.Set<NutritionProgramType>().AnyAsync())
                return;

            context.Set<NutritionProgramType>().AddRange(
                new NutritionProgramType {Name = "روزانه" },
                new NutritionProgramType { Name = "هفتگی" },
                new NutritionProgramType { Name = "ماهانه" }
            );

            await context.SaveChangesAsync(default);
        }
    }

}
