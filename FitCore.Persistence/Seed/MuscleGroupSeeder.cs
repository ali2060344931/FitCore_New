using FitCore.Domain.Entities.TrainingProgram;
using FitCore.Persistence.Common;
using FitCore.Persistence.Contexts;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{
    public class MuscleGroupSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();

            var muscleGroups = new List<MuscleGroup>
            {
                new MuscleGroup { Name = "سینه" },//
                new MuscleGroup { Name = "پشت" },//
                new MuscleGroup { Name = "شانه" },//
                new MuscleGroup { Name = "دوسر بازو" },//
                new MuscleGroup { Name = "سه‌سر بازو" },//
                new MuscleGroup { Name = "ساق پا" },//
                new MuscleGroup { Name = "چهارسر ران" },//
                new MuscleGroup { Name = "همسترینگ" },//
                new MuscleGroup { Name = "باسن (سرینی)" },
                new MuscleGroup { Name = "شکم و میان‌تنه" },//
                new MuscleGroup { Name = "ساعد و مچ" },//
                new MuscleGroup { Name = "تمام بدن" },//
                new MuscleGroup { Name = "هسته" },//
                new MuscleGroup { Name = "ابداکتور" },//
                new MuscleGroup { Name = "ادداکتور" },//
                new MuscleGroup { Name = "تله‌پز" },//
                new MuscleGroup { Name = "کمر پایینی" },//
                new MuscleGroup { Name = "ساعد" },//
                new MuscleGroup { Name = "ساق" },//
                new MuscleGroup { Name = "سرینی" },//
                new MuscleGroup { Name = "ران خارجی" },//
                new MuscleGroup { Name = "ران داخلی" },//
                new MuscleGroup { Name = "کمر" },//
                new MuscleGroup { Name = "پشت بازو" },//
                new MuscleGroup { Name = "جلو بازو" },//
                new MuscleGroup { Name = "سرشانه" },//
                new MuscleGroup { Name = "تله‌پزیوس" },//
                new MuscleGroup { Name = "کمر پایینی" },

                

            };

            await context.SeedIfNotExists(
                muscleGroups,
                muscleGroup => x => x.Name == muscleGroup.Name
            );
        }
    }
}