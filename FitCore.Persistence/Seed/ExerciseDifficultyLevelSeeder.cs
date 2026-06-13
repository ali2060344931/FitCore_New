using FitCore.Domain.Entities.TrainingProgram;
using FitCore.Persistence.Common;
using FitCore.Persistence.Contexts;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{
    public class ExerciseDifficultyLevelSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();

            var levels = new List<ExerciseDifficultyLevel>
            {
                new ExerciseDifficultyLevel { Name = "مبتدی" },
                new ExerciseDifficultyLevel { Name = "متوسط" },
                new ExerciseDifficultyLevel { Name = "پیشرفته" }
            };

            await context.SeedIfNotExists(
                levels,
                level => x => x.Name == level.Name
            );
        }
    }
}