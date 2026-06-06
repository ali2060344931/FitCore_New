using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;
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
    public class ExperienceLevelSeeder:ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();
            var goalTypes = new List<ExperienceLevel>
            {
            new ExperienceLevel { Name = "مبتدی" },
            new ExperienceLevel { Name = "متوسط" },
            new ExperienceLevel { Name = "پیشرفته" },
            };

            await context.SeedIfNotExists(
                goalTypes,
                goal => x => x.Name == goal.Name
            );
        }
    }
}
