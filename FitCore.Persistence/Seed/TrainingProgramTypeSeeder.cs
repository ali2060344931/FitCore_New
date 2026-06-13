using FitCore.Domain.Entities.TrainingProgram;
using FitCore.Persistence.Common;
using FitCore.Persistence.Contexts;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{
    public class TrainingProgramTypeSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();

            var types = new List<TrainingProgramType>
            {
                new TrainingProgramType { Name = "قدرتی" },
                new TrainingProgramType { Name = "بدنسازی (هایپرتروفی)" },
                new TrainingProgramType { Name = "استقامتی" },
                new TrainingProgramType { Name = "فانکشنال" },
                new TrainingProgramType { Name = "کراسفیت" },
                new TrainingProgramType { Name = "کالیستنیکس" },
                new TrainingProgramType { Name = "ترکیبی" }
            };

            await context.SeedIfNotExists(
                types,
                type => x => x.Name == type.Name
            );
        }
    }
}