using System;
using System.Collections.Generic;

namespace FitCore.Application.Services.Member.Queries
{
    public class MemberAnalyticsDto
    {
        // تاریخچه نمودارها
        public List<ChartPointDto> WeightHistory { get; set; } = new List<ChartPointDto>();
        public List<ChartPointDto> FatHistory { get; set; } = new List<ChartPointDto>();
        public List<ChartPointDto> WaistHistory { get; set; } = new List<ChartPointDto>();

        // مقایسه قبل و بعد
        public decimal? FirstWeight { get; set; }
        public decimal? CurrentWeight { get; set; }
        public decimal? WeightChange { get; set; }

        public decimal? FirstFat { get; set; }
        public decimal? CurrentFat { get; set; }
        public decimal? FatChange { get; set; }

        // شاخص های پیشرفته
        public decimal? WaistToHipRatio { get; set; } // نسبت کمر به باسن
        public decimal? WeeklyWeightChangeRate { get; set; } // سرعت تغییر وزن در هفته
        public int? TotalWeeksPassed { get; set; }

        // خلاصه برنامه فعلی
        public NutritionSummaryDto Nutrition { get; set; }
        public TrainingSummaryDto Training { get; set; }
    }

    public class ChartPointDto
    {
        public string Label { get; set; }
        public decimal? Value { get; set; }
    }

    public class NutritionSummaryDto
    {
        public string GoalName { get; set; }
        public decimal AvgDailyCalories { get; set; }
        public decimal AvgDailyProtein { get; set; }
        public decimal AvgDailyCarbs { get; set; }
        public decimal AvgDailyFat { get; set; }

        // درصدهای استاندارد (بر اساس کالری هر ماکرو: پروتئین 4، کربوهیدرات 4، چربی 9)
        public int ProteinPercent { get; set; }
        public int CarbsPercent { get; set; }
        public int FatPercent { get; set; }
    }

    public class TrainingSummaryDto
    {
        public string GoalName { get; set; }
        public int? SessionsPerWeek { get; set; }
        public int TotalActiveDays { get; set; }
        public int TotalSets { get; set; } // حجم تمرین بر اساس تعداد ست
        public int TotalExercises { get; set; } // تنوع حرکات
    }
}