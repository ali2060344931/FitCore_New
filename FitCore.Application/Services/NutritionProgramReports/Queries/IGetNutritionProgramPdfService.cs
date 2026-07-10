using FitCore.Application.Contexts;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;

using Microsoft.EntityFrameworkCore;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using System;
using System.IO;
using System.Linq;

namespace FitCore.Application.Services.NutritionProgramReports.Queries
{
    /// <summary>
    /// گزارش بخش برنامه غذایی
    /// </summary>
    public interface IGetNutritionProgramPdfService
    {
        byte[] Execute(long programId);
    }

    public class GetNutritionProgramPdfService : IGetNutritionProgramPdfService
    {
        private readonly IDataBaseContext _context;

        public GetNutritionProgramPdfService(IDataBaseContext context)
        {
            _context = context;
        }

        public byte[] Execute(long programId)
        {
            var program = _context.NutritionPrograms
                .Include(x => x.Member)
                    .ThenInclude(x => x.AppUser)
                .Include(x => x.Gym)
                .Include(x => x.ProgramType)
                .Include(x => x.GoalType)
                .Include(x => x.Days)
                    .ThenInclude(d => d.Meals)
                        .ThenInclude(m => m.MealType)

                .Include(x => x.Days)
                    .ThenInclude(d => d.Meals)
                        .ThenInclude(m => m.Items)
                            .ThenInclude(i => i.Food)
                .Include(x => x.Days)
                    .ThenInclude(d => d.Meals)
                        .ThenInclude(m => m.Items)
                            .ThenInclude(i => i.UnitType)
                .FirstOrDefault(x => x.Id == programId);

            if (program == null)
                throw new Exception("برنامه غذایی مورد نظر پیدا نشد.");

            // اینجا دقیقاً همان جایی است که Generate استفاده می‌شود
            var pdfBytes = Generate(program);

            return pdfBytes;
        }

        private byte[] Generate(NutritionProgram program)
        {
            var primaryColor = Colors.Green.Medium;
            var lightGray = Colors.Grey.Lighten3;

            return QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(25);

                    // فعال‌سازی جهت‌گیری راست‌به‌چپ برای محتوای صفحه
                    page.ContentFromRightToLeft();

                    page.DefaultTextStyle(x => x
                        .FontFamily("Vazir")
                        .FontSize(11)
                    );

                    // هدر - کاملاً وسط‌چین
                    page.Header().AlignCenter().Column(col =>
                    {
                        if (File.Exists("wwwroot/images/logo.png"))
                        {
                            col.Item().Height(60).Image("wwwroot/images/logo.png");
                            col.Item().PaddingTop(5);
                        }

                        col.Item().Text("برنامه غذایی اختصاصی")
                            .FontSize(20)
                            .Bold()
                            .FontColor(primaryColor).AlignCenter();
                        //
                        col.Item().Text($"نام باشگاه: {program.Gym.Name}")
                            .FontSize(16)
                            .Bold()
                            .FontColor(primaryColor).AlignCenter();

                        col.Item().Text($"عضو: {program.Member?.AppUser?.FullName ?? "-"} | نوع برنامه: {program.ProgramType?.Name ?? "-"} | هدف برنامه: {program.GoalType.Name ?? "-"}").AlignCenter();
                        col.Item().Text($"تاریخ شروع: {program.StartDate} | تاریخ پایان: {program.EndDate}").AlignCenter();
                    });

                    // بدنه - راست‌چین
                    page.Content().PaddingVertical(10).Column(column =>
                    {
                        foreach (var day in program.Days.OrderBy(x => x.DayNumber))
                        {
                            column.Item().PaddingTop(15).Element(dayContainer =>
                            {
                                dayContainer
                                    .Background(lightGray)
                                    .Padding(10)
                                    .Column(dayColumn =>
                                    {
                                        dayColumn.Item()
                                            .Text($"روز {day.DayNumber} - {day.Title}")
                                            .FontSize(16)
                                            .Bold()
                                            .FontColor(primaryColor)
                                            .AlignRight();

                                        foreach (var meal in day.Meals.OrderBy(x => x.SortOrder))
                                        {
                                            var totalCalories = meal.Items.Sum(i => i.Calories);
                                            var totalProtein = meal.Items.Sum(i => i.Protein);
                                            var totalCarbs = meal.Items.Sum(i => i.Carbohydrate);
                                            var totalFat = meal.Items.Sum(i => i.Fat);

                                            dayColumn.Item().PaddingTop(10).Column(mealCol =>
                                            {
                                                mealCol.Item()
                                                    .Text($"وعده: {meal.MealType?.Name ?? "-"}")
                                                    .FontSize(14)
                                                    .Bold()
                                                    .AlignRight();

                                                mealCol.Item().PaddingTop(5).Table(table =>
                                                {
                                                    // تعریف ستون‌ها (معکوس برای RTL)
                                                    table.ColumnsDefinition(columns =>
                                                    {
                                                        columns.ConstantColumn(30);      // ردیف
                                                        columns.RelativeColumn(3);       // غذا (می‌تواند تا جایی که ممکن است بزرگ شود)
                                                        columns.ConstantColumn(60);      // مقدار (عرض محدود و ثابت - مثلا 60 پیکسل)
                                                        columns.ConstantColumn(60);      // واحد (عرض محدود و ثابت - مثلا 60 پیکسل)
                                                        columns.RelativeColumn(2);//توضیحات
                                                        columns.RelativeColumn(1); // چربی
                                                        columns.RelativeColumn(1); // کربوهیدرات
                                                        columns.RelativeColumn(1); // پروتئین
                                                        columns.RelativeColumn(1); // کالری

                                                    });

                                                    table.Header(header =>
                                                    {
                                                        header.Cell().Element(CellStyleR).Text("ردیف");
                                                        header.Cell().Element(CellStyleR).Text("غذا");
                                                        header.Cell().Element(CellStyleR).Text("مقدار");
                                                        header.Cell().Element(CellStyleR).Text("واحد");
                                                        header.Cell().Element(CellStyleR).Text("توضیحات");
                                                        header.Cell().Element(CellStyleR).Text("چربی");
                                                        header.Cell().Element(CellStyleR).Text("کربوهیدرات");
                                                        header.Cell().Element(CellStyleR).Text("پروتئین");
                                                        header.Cell().Element(CellStyleR).Text("کالری");

                                                    });
                                                    int rowNumber = 1;
                                                    foreach (var item in meal.Items)
                                                    {
                                                        table.Cell().Element(CellBodyU).Text(rowNumber.ToString()).FontSize(7).SemiBold();
                                                        table.Cell().Element(CellStyle).Text(item.Food?.Title ?? "-").FontSize(11).SemiBold();
                                                        table.Cell().Element(CellBody).Text($"{item.Amount:0.##} ");
                                                        table.Cell().Element(CellBodyU).Text($"{item.UnitType?.Name}");
                                                        table.Cell().Element(CellBodyU).Text($"{item.Description}");
                                                        table.Cell().Element(CellBody).Text(Math.Round((decimal)item.Fat, 0).ToString());
                                                        table.Cell().Element(CellBody).Text(Math.Round((decimal)item.Carbohydrate, 0).ToString());
                                                        table.Cell().Element(CellBody).Text(Math.Round((decimal)item.Protein, 0).ToString());
                                                        table.Cell().Element(CellBody).Text(Math.Round((decimal)item.Calories, 0).ToString());
                                                        rowNumber++;
                                                    }

                                                    table.Cell().ColumnSpan(5).Element(CellStyle).Text("جمع");
                                                    table.Cell().Element(CellStyle).Text(Math.Round((decimal)totalFat, 0).ToString());
                                                    table.Cell().Element(CellStyle).Text(Math.Round((decimal)totalCarbs, 0).ToString());
                                                    table.Cell().Element(CellStyle).Text(Math.Round((decimal)totalProtein, 0).ToString());
                                                    table.Cell().Element(CellStyle).Text(Math.Round((decimal)totalCalories, 0).ToString());

                                                });
                                            });
                                        }
                                    });
                            });
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("صفحه ");
                        text.CurrentPageNumber();
                        text.Span(" از ");
                        text.TotalPages();


                    });
                });
            }).GeneratePdf();

            // استایل‌های کمکی
            IContainer CellStyle(IContainer container)
                => container
                    .Padding(5)
                    .Background(Colors.Green.Lighten4)
                    .Border(1)
                    .BorderColor(Colors.Grey.Lighten1)
                    .AlignRight()
                    .DefaultTextStyle(x => x.FontSize(10)) // اندازه فونت
                    ; // تیترها وسط‌چین باشند بهتر است، اگر خواستی راست‌چین کنی AlignRight کن
            IContainer CellStyleR(IContainer container)
                => container
                    .Padding(5)
                    .Background(Colors.Green.Lighten4)
                    .Border(1)
                    .BorderColor(Colors.Grey.Lighten1)
                    .AlignCenter()
                    .DefaultTextStyle(x => x.FontSize(7)) // اندازه فونت
                    ; // تیترها وسط‌چین باشند بهتر است، اگر خواستی راست‌چین کنی AlignRight کن

            IContainer CellBody(IContainer container)
                => container
                    .Padding(5)
                    .Border(1)
                    .BorderColor(Colors.Grey.Lighten2)
                    .AlignCenter()
                    .DefaultTextStyle(x => x.FontSize(10)) // اندازه فونت
                    ; // محتوای سلول‌ها راست‌چین

            IContainer CellBodyU(IContainer container)
                => container
                    .Padding(5)
                    .Border(1)
                    .BorderColor(Colors.Grey.Lighten2)
                    .AlignCenter()
                    .DefaultTextStyle(x => x.FontSize(7)) // اندازه فونت
                    ; // محتوای سلول‌ها راست‌چین
        }
    }
}
