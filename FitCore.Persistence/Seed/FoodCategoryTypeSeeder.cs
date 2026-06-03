using FitCore.Domain.Entities.NutritionProgram.Food;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;
using FitCore.Persistence.Common;
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

    public class FoodCategoryTypeSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();
            var data = new List<FoodCategoryType>
            {
                new FoodCategoryType { Name = "پروتئین" },
                new FoodCategoryType {  Name = "کربوهیدرات" },
                new FoodCategoryType {  Name = "چربی" },
                new FoodCategoryType {  Name = "میوه" },
                new FoodCategoryType {  Name = "سبزیجات" },
                new FoodCategoryType {  Name = "نوشیدنی" },
                new FoodCategoryType {  Name = "لبنیات" },
                new FoodCategoryType {  Name = "حبوبات" },
                new FoodCategoryType {  Name = "قند و شیرینی‌" },
                new FoodCategoryType {  Name = "چاشنی‌ها و افزودنی‌ها" },
                new FoodCategoryType {  Name = "غذای فرآوری‌شده" },
                new FoodCategoryType {  Name = "سبزیجات" }
            };

            await context.SeedIfNotExists(
                data,
                goal => x => x.Name == goal.Name
            );


        }
    }

}
