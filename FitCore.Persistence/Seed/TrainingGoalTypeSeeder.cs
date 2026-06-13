using FitCore.Domain.Entities.TrainingProgram;
using FitCore.Persistence.Common;
using FitCore.Persistence.Contexts;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{
    public class TrainingGoalTypeSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();

            var goals = new List<TrainingGoalType>
            {
                new TrainingGoalType { Name = "افزایش حجم عضلانی" },
                new TrainingGoalType { Name = "کاهش چربی (کات)" },
                new TrainingGoalType { Name = "افزایش قدرت" },
                new TrainingGoalType { Name = "تناسب اندام عمومی" },
                new TrainingGoalType { Name = "آمادگی استقامتی" },
                new TrainingGoalType { Name = "ریکاوری / توانبخشی" }
            };

            await context.SeedIfNotExists(
                goals,
                goal => x => x.Name == goal.Name
            );
        }
    }
}