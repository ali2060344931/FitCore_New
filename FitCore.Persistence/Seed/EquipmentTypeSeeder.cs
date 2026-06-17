using FitCore.Domain.Entities.TrainingProgram;
using FitCore.Persistence.Common;
using FitCore.Persistence.Contexts;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{
    public class EquipmentTypeSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();

            var equipmentTypes = new List<EquipmentType>
            {
                new EquipmentType { Name = "هالتر (باربل)" },
                new EquipmentType { Name = "هالتر" },
                new EquipmentType { Name = "دمبل" },
                new EquipmentType { Name = "دستگاه" },
                new EquipmentType { Name = "کابل" },
                new EquipmentType { Name = "وزن بدن" },
                new EquipmentType { Name = "کش تمرینی" },
                new EquipmentType { Name = "کتل‌بل" },
                new EquipmentType { Name = "باند مقاومتی" },
                new EquipmentType { Name = "طناب" },
                new EquipmentType { Name = "ابزار دیگر" },
                new EquipmentType { Name = "TRX" },
                new EquipmentType { Name = "توپ طبی" },
                new EquipmentType { Name = "توپ سوئیسی" },
                new EquipmentType { Name = "سایر" },
            };

            await context.SeedIfNotExists(
                equipmentTypes,
                equipmentType => x => x.Name == equipmentType.Name
            );
        }
    }
}