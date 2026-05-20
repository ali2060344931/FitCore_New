using FitCore.Domain.Entities.Setings;
using FitCore.Persistence.Contexts;
using FitCore.Persistence.Seed;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;
using System.Threading.Tasks;


/// <summary>
/// ثبت اولیه تنظیمات
/// </summary>
public class SettingsSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<DataBaseContext>();

        if (!context.Setings.Any())
        {
            context.Setings.Add(new Setings
            {
                Code = "01",
                TextFilde = "نرم افزار فیتکو",
                Email = "info@fitcore.com",
                Phone = "09111111111"
            });

            await context.SaveChangesAsync();
        }
    }
}
