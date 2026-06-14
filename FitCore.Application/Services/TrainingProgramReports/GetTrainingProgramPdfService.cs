using FitCore.Application.Contexts;
using FitCore.Domain.Entities.TrainingProgram;

using Microsoft.EntityFrameworkCore;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using System;
using System.IO;
using System.Linq;

namespace FitCore.Application.Services.TrainingProgramReports.Queries
{
    public interface IGetTrainingProgramPdfService
    {
        byte[] Execute(long programId);
    }

    public class GetTrainingProgramPdfService : IGetTrainingProgramPdfService
    {
        private readonly IDataBaseContext _context;

        public GetTrainingProgramPdfService(IDataBaseContext context)
        {
            _context = context;
        }

        public byte[] Execute(long programId)
        {
            var program = _context.TrainingPrograms
                .Include(x => x.Member)
                    .ThenInclude(x => x.AppUser)
                .Include(x => x.Gym)
                .Include(x => x.TrainingProgramType)
                .Include(x => x.TrainingGoalType)
                .Include(x => x.Days)
                    .ThenInclude(d => d.DayType)
                .Include(x => x.Days)
                    .ThenInclude(d => d.ExerciseItems)
                        .ThenInclude(e => e.Exercise)
                            .ThenInclude(ex => ex.PrimaryMuscleGroup)
                .FirstOrDefault(x => x.Id == programId && !x.IsRemoved);

            if (program == null)
                throw new Exception("برنامه تمرینی مورد نظر پیدا نشد.");

            return Generate(program);
        }

        private byte[] Generate(TrainingProgram program)
        {
            var primaryColor = Colors.Green.Medium;
            var lightGray    = Colors.Grey.Lighten3;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(25);
                    page.ContentFromRightToLeft();

                    page.DefaultTextStyle(x => x
                        .FontFamily("Vazir")
                        .FontSize(11));

                    //====================================
                    // هدر
                    //====================================
                    page.Header().AlignCenter().Column(col =>
                    {
                        if (File.Exists("wwwroot/images/logo.png"))
                        {
                            col.Item().Height(60).Image("wwwroot/images/logo.png");
                            col.Item().PaddingTop(5);
                        }

                        col.Item().Text("برنامه تمرینی اختصاصی")
                            .FontSize(20).Bold()
                            .FontColor(primaryColor).AlignCenter();

                        col.Item().Text($"نام باشگاه: {program.Gym?.Name ?? "-"}")
                            .FontSize(16).Bold()
                            .FontColor(primaryColor).AlignCenter();

                        col.Item().Text(
                            $"ورزشکار: {program.Member?.AppUser?.FullName ?? "-"} | " +
                            $"نوع برنامه: {program.TrainingProgramType?.Name ?? "-"} | " +
                            $"هدف: {program.TrainingGoalType?.Name ?? "-"}")
                            .AlignCenter();

                        col.Item().Text(
                            $"تاریخ شروع: {program.StartDate} | تاریخ پایان: {program.EndDate}")
                            .AlignCenter();

                        if (program.SessionsPerWeek.HasValue)
                        {
                            col.Item().Text($"تعداد جلسات در هفته: {program.SessionsPerWeek}")
                                .AlignCenter();
                        }
                    });

                    //====================================
                    // بدنه
                    //====================================
                    page.Content().PaddingVertical(10).Column(column =>
                    {
                        var days = program.Days?
                            .Where(d => !d.IsRemoved)
                            .OrderBy(d => d.SortOrder)
                            .ThenBy(d => d.DayNumber)
                            .ToList();

                        if (days == null || !days.Any())
                        {
                            column.Item().Text("هنوز هیچ روز تمرینی ثبت نشده است.")
                                .AlignCenter().FontColor(Colors.Grey.Medium);
                            return;
                        }

                        foreach (var day in days)
                        {
                            column.Item().PaddingTop(15).Element(dayContainer =>
                            {
                                dayContainer
                                    .Background(lightGray)
                                    .Padding(10)
                                    .Column(dayCol =>
                                    {
                                        // عنوان روز
                                        dayCol.Item()
                                            .Text($"روز {day.DayNumber} — {day.Title ?? ""} ({day.DayType?.Name ?? ""})")
                                            .FontSize(15).Bold()
                                            .FontColor(primaryColor)
                                            .AlignRight();

                                        if (day.DurationMinutes.HasValue)
                                        {
                                            dayCol.Item()
                                                .Text($"مدت زمان: {day.DurationMinutes} دقیقه")
                                                .FontSize(10).FontColor(Colors.Grey.Darken1)
                                                .AlignRight();
                                        }

                                        if (!string.IsNullOrWhiteSpace(day.Description))
                                        {
                                            dayCol.Item().PaddingTop(4)
                                                .Text(day.Description)
                                                .FontSize(10).Italic()
                                                .AlignRight();
                                        }

                                        // جدول حرکات
                                        var items = day.ExerciseItems?
                                            .Where(e => !e.IsRemoved)
                                            .OrderBy(e => e.SortOrder)
                                            .ToList();

                                        if (items == null || !items.Any())
                                        {
                                            dayCol.Item().PaddingTop(6)
                                                .Text("هیچ حرکتی ثبت نشده است.")
                                                .FontSize(10).FontColor(Colors.Grey.Medium)
                                                .AlignRight();
                                            return;
                                        }

                                        dayCol.Item().PaddingTop(8).Table(table =>
                                        {
                                            table.ColumnsDefinition(cols =>
                                            {
                                                cols.ConstantColumn(28);  // ردیف
                                                cols.RelativeColumn(3);   // نام حرکت
                                                cols.RelativeColumn(2);   // گروه عضلانی
                                                cols.ConstantColumn(35);  // ست
                                                cols.ConstantColumn(45);  // تکرار
                                                cols.ConstantColumn(50);  // وزن
                                                cols.ConstantColumn(50);  // استراحت
                                                cols.RelativeColumn(3);   // یادداشت
                                            });

                                            // هدر جدول
                                            table.Header(header =>
                                            {
                                                header.Cell().Element(HeaderCell).Text("#");
                                                header.Cell().Element(HeaderCell).Text("نام حرکت");
                                                header.Cell().Element(HeaderCell).Text("گروه عضلانی");
                                                header.Cell().Element(HeaderCell).Text("ست");
                                                header.Cell().Element(HeaderCell).Text("تکرار");
                                                header.Cell().Element(HeaderCell).Text("وزن(kg)");
                                                header.Cell().Element(HeaderCell).Text("استراحت(s)");
                                                header.Cell().Element(HeaderCell).Text("یادداشت مربی");
                                            });

                                            // ردیف‌های حرکات
                                            int rowNum = 1;
                                            foreach (var item in items)
                                            {
                                                table.Cell().Element(SmallCell).Text(rowNum.ToString()).FontSize(8);
                                                table.Cell().Element(BodyCell).Text(item.Exercise?.Name ?? "-").FontSize(10).SemiBold();
                                                table.Cell().Element(SmallCell).Text(item.Exercise?.PrimaryMuscleGroup?.Name ?? "-").FontSize(9);
                                                table.Cell().Element(CenterCell).Text(item.Sets?.ToString() ?? "-").FontSize(10);
                                                table.Cell().Element(CenterCell).Text(item.Reps ?? "-").FontSize(10);
                                                table.Cell().Element(CenterCell).Text(item.WeightKg?.ToString("0.##") ?? "-").FontSize(10);
                                                table.Cell().Element(CenterCell).Text(item.RestSeconds?.ToString() ?? "-").FontSize(9);
                                                table.Cell().Element(SmallCell).Text(item.CoachNote ?? "-").FontSize(9);
                                                rowNum++;
                                            }
                                        });
                                    });
                            });
                        }
                    });

                    //====================================
                    // فوتر
                    //====================================
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
            IContainer HeaderCell(IContainer c) => c
                .Padding(5)
                .Background(Colors.Green.Lighten4)
                .Border(1).BorderColor(Colors.Grey.Lighten1)
                .AlignCenter()
                .DefaultTextStyle(x => x.FontSize(8).Bold());

            IContainer BodyCell(IContainer c) => c
                .Padding(5)
                .Border(1).BorderColor(Colors.Grey.Lighten2)
                .AlignRight()
                .DefaultTextStyle(x => x.FontSize(10));

            IContainer SmallCell(IContainer c) => c
                .Padding(5)
                .Border(1).BorderColor(Colors.Grey.Lighten2)
                .AlignCenter()
                .DefaultTextStyle(x => x.FontSize(8));

            IContainer CenterCell(IContainer c) => c
                .Padding(5)
                .Border(1).BorderColor(Colors.Grey.Lighten2)
                .AlignCenter()
                .DefaultTextStyle(x => x.FontSize(10));
        }
    }
}
