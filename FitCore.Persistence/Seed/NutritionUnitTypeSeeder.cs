using FitCore.Domain.Entities.NutritionProgram.Food;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;
using FitCore.Persistence.Common;
using FitCore.Persistence.Contexts;
using FitCore.Persistence.Seed;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class NutritionUnitTypeSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider serviceProvider)
    {

        var context = serviceProvider.GetRequiredService<DataBaseContext>();
        var data = new List<NutritionUnitType>
            {

            new NutritionUnitType {Name = "گرم" },
            new NutritionUnitType {  Name = "عدد" },
            new NutritionUnitType {  Name = "قاشق چای‌خوری" },
            new NutritionUnitType {  Name = "پیمانه" },
            new NutritionUnitType {  Name = "میلی‌گرم" },
            new NutritionUnitType {  Name = "کیلوگرم" },
            new NutritionUnitType {  Name = "میلی‌لیتر" },
            new NutritionUnitType {  Name = "لیتر" },
            new NutritionUnitType {  Name = "سی‌سی" },
            new NutritionUnitType {  Name = "قطره" },
            new NutritionUnitType {  Name = "قاشق مرباخوری" },
            new NutritionUnitType {  Name = "قاشق دسرخوری" },
            new NutritionUnitType {  Name = "قاشق غذاخوری" },
            new NutritionUnitType {  Name = "نصف پیمانه" },
            new NutritionUnitType {  Name = "یک‌سوم پیمانه" },
            new NutritionUnitType {  Name = "یک‌چهارم پیمانه" },
            new NutritionUnitType {  Name = "لیوان" },
            new NutritionUnitType {  Name = "استکان" },
            new NutritionUnitType {  Name = "فنجان" },
            new NutritionUnitType {  Name = "ماگ" },
            new NutritionUnitType {  Name = "پیاله" },
            new NutritionUnitType {  Name = "کاسه" },
            new NutritionUnitType {  Name = "بشقاب" },
            new NutritionUnitType {  Name = "نعلبکی" },
            new NutritionUnitType {  Name = "ملاقه" },
            new NutritionUnitType {  Name = "کفگیر" },
            new NutritionUnitType {  Name = "کف دست" },
            new NutritionUnitType {  Name = "مشت" },
            new NutritionUnitType {  Name = "نیم‌مشت" },
            new NutritionUnitType {  Name = "نوک انگشت" },
            new NutritionUnitType {  Name = "بند انگشت" },
            new NutritionUnitType {  Name = "تکه" },
            new NutritionUnitType {  Name = "نصف عدد" },
            new NutritionUnitType {  Name = "یک‌چهارم عدد" },
            new NutritionUnitType {  Name = "برش" },
            new NutritionUnitType {  Name = "قطعه" },
            new NutritionUnitType {  Name = "حبه" },
            new NutritionUnitType {  Name = "پر" },
            new NutritionUnitType {  Name = "پرس" },
            new NutritionUnitType {  Name = "بسته" },
            new NutritionUnitType {  Name = "پاکت" },
            new NutritionUnitType {  Name = "بطری" },
            new NutritionUnitType {  Name = "قوطی" },
            new NutritionUnitType {  Name = "کنسرو" },
            new NutritionUnitType {  Name = "واحد" },
            new NutritionUnitType {  Name = "سهم" }
            };

        await context.SeedIfNotExists(
            data,
            goal => x => x.Name == goal.Name
        );
        /*
        var context = serviceProvider.GetRequiredService<DataBaseContext>();

        // اگر قبلاً داده‌ها وارد شده‌اند، تکراری وارد نکن
        if (await context.Set<NutritionUnitType>().AnyAsync())
            return;

        context.Set<NutritionUnitType>().AddRange(
            new NutritionUnitType {Name = "گرم" },
            new NutritionUnitType {  Name = "عدد" },
            new NutritionUnitType {  Name = "قاشق چای‌خوری" },
            new NutritionUnitType {  Name = "پیمانه" },
            new NutritionUnitType {  Name = "میلی‌گرم" },
            new NutritionUnitType {  Name = "کیلوگرم" },
            new NutritionUnitType {  Name = "میلی‌لیتر" },
            new NutritionUnitType {  Name = "لیتر" },
            new NutritionUnitType {  Name = "سی‌سی" },
            new NutritionUnitType {  Name = "قطره" },
            new NutritionUnitType {  Name = "قاشق مرباخوری" },
            new NutritionUnitType {  Name = "قاشق دسرخوری" },
            new NutritionUnitType {  Name = "قاشق غذاخوری" },
            new NutritionUnitType {  Name = "نصف پیمانه" },
            new NutritionUnitType {  Name = "یک‌سوم پیمانه" },
            new NutritionUnitType {  Name = "یک‌چهارم پیمانه" },
            new NutritionUnitType {  Name = "لیوان" },
            new NutritionUnitType {  Name = "استکان" },
            new NutritionUnitType {  Name = "فنجان" },
            new NutritionUnitType {  Name = "ماگ" },
            new NutritionUnitType {  Name = "پیاله" },
            new NutritionUnitType {  Name = "کاسه" },
            new NutritionUnitType {  Name = "بشقاب" },
            new NutritionUnitType {  Name = "نعلبکی" },
            new NutritionUnitType {  Name = "ملاقه" },
            new NutritionUnitType {  Name = "کفگیر" },
            new NutritionUnitType {  Name = "کف دست" },
            new NutritionUnitType {  Name = "مشت" },
            new NutritionUnitType {  Name = "نیم‌مشت" },
            new NutritionUnitType {  Name = "نوک انگشت" },
            new NutritionUnitType {  Name = "بند انگشت" },
            new NutritionUnitType {  Name = "تکه" },
            new NutritionUnitType {  Name = "نصف عدد" },
            new NutritionUnitType {  Name = "یک‌چهارم عدد" },
            new NutritionUnitType {  Name = "برش" },
            new NutritionUnitType {  Name = "قطعه" },
            new NutritionUnitType {  Name = "حبه" },
            new NutritionUnitType {  Name = "پر" },
            new NutritionUnitType {  Name = "پرس" },
            new NutritionUnitType {  Name = "بسته" },
            new NutritionUnitType {  Name = "پاکت" },
            new NutritionUnitType {  Name = "بطری" },
            new NutritionUnitType {  Name = "قوطی" },
            new NutritionUnitType {  Name = "کنسرو" },
            new NutritionUnitType {  Name = "واحد" },
            new NutritionUnitType {  Name = "سهم" }
        );

        await context.SaveChangesAsync(default);
        */
    }
}
