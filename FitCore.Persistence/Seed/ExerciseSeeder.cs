using FitCore.Domain.Entities.TrainingProgram;
using FitCore.Persistence.Common;
using FitCore.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed;

public class ExerciseSeeder : ISeeder
{

    public async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<DataBaseContext>();
        // بارگذاری IDهای lookup tableها
        var muscleGroups = await context.Set<MuscleGroup>().ToListAsync();
        var equipmentTypes = await context.Set<EquipmentType>().ToListAsync();
        var difficultyLevels = await context.Set<ExerciseDifficultyLevel>().ToListAsync();

         int MG(string name)
    {
        var found = muscleGroups.FirstOrDefault(x => x.Name == name);
        if (found == null)
            throw new InvalidOperationException(
                $"MuscleGroup '{name}' not found. Available: {string.Join(", ", muscleGroups.Select(x => x.Name))}");
        return found.Id;
    }

     int EQ(string name)
    {
        var found = equipmentTypes.FirstOrDefault(x => x.Name == name);
        if (found == null)
            throw new InvalidOperationException(
                $"EquipmentType '{name}' not found. Available: {string.Join(", ", equipmentTypes.Select(x => x.Name))}");
        return found.Id;
    }

     int DL(string name)
    {
        var found = difficultyLevels.FirstOrDefault(x => x.Name == name);
        if (found == null)
            throw new InvalidOperationException(
                $"DifficultyLevel '{name}' not found. Available: {string.Join(", ", difficultyLevels.Select(x => x.Name))}");
        return found.Id;
    }






    var exercises = new List<Exercise>
        {
            // ══════════════════════════════════════════
            // سینه – Chest
            // ══════════════════════════════════════════
            new() { Name="پرس سینه هالتر",                 EnglishName="Barbell Bench Press",                PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="پرس سینه دمبل",                  EnglishName="Dumbbell Bench Press",               PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="پرس سینه بالا‌سینه هالتر",       EnglishName="Incline Barbell Bench Press",        PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="پرس سینه بالا‌سینه دمبل",        EnglishName="Incline Dumbbell Bench Press",       PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="پرس سینه پایین‌سینه هالتر",      EnglishName="Decline Barbell Bench Press",        PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="پرس سینه پایین‌سینه دمبل",       EnglishName="Decline Dumbbell Bench Press",       PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="فلای دمبل",                       EnglishName="Dumbbell Fly",                       PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="فلای بالاسینه دمبل",              EnglishName="Incline Dumbbell Fly",               PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="کراس‌اور کابل",                   EnglishName="Cable Crossover",                    PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پرس سینه دستگاه",                 EnglishName="Machine Chest Press",                PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="فلای دستگاه (پک‌دک)",             EnglishName="Pec Deck Fly",                       PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="شنا سوئدی",                       EnglishName="Push-Up",                            PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="شنا با دست جمع",                  EnglishName="Diamond Push-Up",                    PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="شنا پایین‌سینه روی میز",          EnglishName="Decline Push-Up",                    PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="دیپس موازی",                      EnglishName="Chest Dip",                          PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="پول‌اور دمبل",                    EnglishName="Dumbbell Pullover",                  PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="پرس سینه هالتر دست جمع",         EnglishName="Close-Grip Bench Press",             PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="فلای کابل بالاسینه",              EnglishName="High Cable Fly",                     PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="فلای کابل پایین‌سینه",            EnglishName="Low Cable Fly",                      PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پرس سینه هالتر بالاسینه دست باز",EnglishName="Wide-Grip Incline Bench Press",      PrimaryMuscleGroupId=MG("سینه"),             EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"),  IsActive=true },

            // ══════════════════════════════════════════
            // پشت – Back
            // ══════════════════════════════════════════
            new() { Name="ددلیفت هالتر",                    EnglishName="Barbell Deadlift",                   PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="بارفیکس",                         EnglishName="Pull-Up",                            PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="بارفیکس دست کج (چین‌آپ)",        EnglishName="Chin-Up",                            PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="زیربغل هالتر خم",                 EnglishName="Bent-Over Barbell Row",              PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="زیربغل دمبل تک‌دست",              EnglishName="Single-Arm Dumbbell Row",            PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="زیربغل دستگاه نشسته",             EnglishName="Seated Cable Row",                   PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="لت پول‌داون دست باز",             EnglishName="Wide-Grip Lat Pulldown",             PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="لت پول‌داون دست جمع",             EnglishName="Close-Grip Lat Pulldown",            PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="تی‌بار رو",                       EnglishName="T-Bar Row",                          PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="ددلیفت رومانیایی",                EnglishName="Romanian Deadlift",                  PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="هایپراکستنشن",                    EnglishName="Back Extension",                     PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="زیربغل دستگاه هامر",              EnglishName="Hammer Strength Row",                PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پول‌اور کابل",                    EnglishName="Cable Pullover",                     PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="فیس‌پول",                         EnglishName="Face Pull",                          PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="ردفای هالتر ایستاده",             EnglishName="Upright Row",                        PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="زیربغل معکوس دستگاه",             EnglishName="Reverse Grip Lat Pulldown",          PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="ددلیفت سوموم",                    EnglishName="Sumo Deadlift",                      PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="رینگ رو",                         EnglishName="Ring Row",                           PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="کیبل رو یک‌طرفه",                EnglishName="Single-Arm Cable Row",               PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="ددلیفت دمبل",                     EnglishName="Dumbbell Deadlift",                  PrimaryMuscleGroupId=MG("پشت"),              EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),    IsActive=true },

            // ══════════════════════════════════════════
            // شانه – Shoulders
            // ══════════════════════════════════════════
            new() { Name="پرس سرشانه هالتر ایستاده",        EnglishName="Standing Barbell Overhead Press",    PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="پرس سرشانه دمبل نشسته",           EnglishName="Seated Dumbbell Shoulder Press",     PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="نشر جانبی دمبل",                  EnglishName="Dumbbell Lateral Raise",             PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="نشر جلو دمبل",                    EnglishName="Dumbbell Front Raise",               PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="نشر خم دمبل",                     EnglishName="Bent-Over Dumbbell Rear Delt Raise", PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پرس سرشانه دستگاه",               EnglishName="Machine Shoulder Press",             PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="نشر جانبی کابل",                  EnglishName="Cable Lateral Raise",                PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="آرنولد پرس",                      EnglishName="Arnold Press",                       PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="پوش پرس",                         EnglishName="Push Press",                         PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="نشر خم کابل",                     EnglishName="Cable Rear Delt Fly",                PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="شراگ هالتر",                      EnglishName="Barbell Shrug",                      PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="شراگ دمبل",                       EnglishName="Dumbbell Shrug",                     PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پرس هالتر پشت گردن",              EnglishName="Behind-the-Neck Barbell Press",      PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="نشر جلو هالتر",                   EnglishName="Barbell Front Raise",                PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="ردفای هالتر",                     EnglishName="Barbell Upright Row",                PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="نشر جانبی دمبل نشسته",            EnglishName="Seated Dumbbell Lateral Raise",      PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پرس سرشانه کتل‌بل",               EnglishName="Kettlebell Overhead Press",          PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="هندستند پوش‌آپ",                  EnglishName="Handstand Push-Up",                  PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="نشر جانبی کش تمرینی",             EnglishName="Band Lateral Raise",                 PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پرس سرشانه یک‌دست کابل",          EnglishName="Single-Arm Cable Overhead Press",    PrimaryMuscleGroupId=MG("شانه"),             EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("متوسط"),    IsActive=true },

            // ══════════════════════════════════════════
            // دوسر بازو – Biceps
            // ══════════════════════════════════════════
            new() { Name="جلوبازو هالتر ایستاده",           EnglishName="Standing Barbell Curl",              PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="جلوبازو دمبل ایستاده",            EnglishName="Standing Dumbbell Curl",             PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="جلوبازو چکشی دمبل",               EnglishName="Hammer Curl",                        PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="جلوبازو تمرکزی دمبل",             EnglishName="Concentration Curl",                 PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="جلوبازو لاری هالتر",              EnglishName="Barbell Preacher Curl",              PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="جلوبازو لاری دمبل",               EnglishName="Dumbbell Preacher Curl",             PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="جلوبازو کابل ایستاده",             EnglishName="Standing Cable Curl",                PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="جلوبازو هالتر EZ",                EnglishName="EZ-Bar Curl",                        PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="جلوبازو کابل یک‌دست",             EnglishName="Single-Arm Cable Curl",              PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="جلوبازو دستگاه",                  EnglishName="Machine Bicep Curl",                 PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="جلوبازو کش تمرینی",               EnglishName="Band Bicep Curl",                    PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="جلوبازو کابل بالا",               EnglishName="High Cable Curl",                    PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="جلوبازو چرخشی دمبل",              EnglishName="Supinating Dumbbell Curl",           PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="جلوبازو زاویه ۲۱",                EnglishName="21s Barbell Curl",                   PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="جلوبازو هالتر معکوس",              EnglishName="Reverse Barbell Curl",               PrimaryMuscleGroupId=MG("دوسر بازو"),        EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("مبتدی"),    IsActive=true },

            // ══════════════════════════════════════════
            // سه‌سر بازو – Triceps
            // ══════════════════════════════════════════
            new() { Name="پشت بازو کابل (پوش‌داون)",        EnglishName="Cable Tricep Pushdown",              PrimaryMuscleGroupId=MG("سه‌سر بازو"),       EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پشت بازو کابل طناب",              EnglishName="Rope Tricep Pushdown",               PrimaryMuscleGroupId=MG("سه‌سر بازو"),       EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پشت بازو دمبل سرشانه",            EnglishName="Overhead Dumbbell Tricep Extension", PrimaryMuscleGroupId=MG("سه‌سر بازو"),       EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="اسکال‌کراشر هالتر",               EnglishName="Skull Crusher (Barbell)",            PrimaryMuscleGroupId=MG("سه‌سر بازو"),       EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="اسکال‌کراشر EZ",                  EnglishName="Skull Crusher (EZ-Bar)",             PrimaryMuscleGroupId=MG("سه‌سر بازو"),       EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="دیپس پشت بازو نیمکت",             EnglishName="Bench Dip",                          PrimaryMuscleGroupId=MG("سه‌سر بازو"),       EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پشت بازو هالتر ایستاده",          EnglishName="Standing Barbell Overhead Extension",PrimaryMuscleGroupId=MG("سه‌سر بازو"),       EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پشت بازو دستگاه",                 EnglishName="Machine Tricep Extension",           PrimaryMuscleGroupId=MG("سه‌سر بازو"),       EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پشت بازو کابل معکوس",             EnglishName="Reverse Grip Cable Pushdown",        PrimaryMuscleGroupId=MG("سه‌سر بازو"),       EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پشت بازو کابل یک‌دست",            EnglishName="Single-Arm Cable Extension",         PrimaryMuscleGroupId=MG("سه‌سر بازو"),       EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="کیک‌بک پشت بازو دمبل",            EnglishName="Dumbbell Tricep Kickback",           PrimaryMuscleGroupId=MG("سه‌سر بازو"),       EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پرس نزدیک هالتر (CG Bench)",      EnglishName="Close-Grip Barbell Bench Press",     PrimaryMuscleGroupId=MG("سه‌سر بازو"),       EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="پشت بازو دمبل خوابیده",           EnglishName="Lying Dumbbell Tricep Extension",    PrimaryMuscleGroupId=MG("سه‌سر بازو"),       EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پشت بازو کش تمرینی",              EnglishName="Band Tricep Pushdown",               PrimaryMuscleGroupId=MG("سه‌سر بازو"),       EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),    IsActive=true },

            // ══════════════════════════════════════════
            // چهارسر ران – Quadriceps
            // ══════════════════════════════════════════
            new() { Name="اسکات هالتر",                     EnglishName="Barbell Back Squat",                 PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="اسکات جلو هالتر",                 EnglishName="Barbell Front Squat",                PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="اسکات دمبل",                      EnglishName="Dumbbell Squat",                     PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پرس پا دستگاه",                   EnglishName="Leg Press",                          PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="اکستنشن پا دستگاه",               EnglishName="Leg Extension",                      PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="لانج هالتر",                      EnglishName="Barbell Lunge",                      PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="لانج دمبل",                       EnglishName="Dumbbell Lunge",                     PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="اسکات وزن بدن",                   EnglishName="Bodyweight Squat",                   PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="اسکات هاک",                       EnglishName="Hack Squat",                         PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="اسکات بلغاری",                    EnglishName="Bulgarian Split Squat",              PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="استپ‌آپ دمبل",                    EnglishName="Dumbbell Step-Up",                   PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="وال‌ست (نشست دیواری)",            EnglishName="Wall Sit",                           PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="اسکات گابلت کتل‌بل",              EnglishName="Kettlebell Goblet Squat",            PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="لانج معکوس دمبل",                 EnglishName="Reverse Dumbbell Lunge",             PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="اسکات سومو دمبل",                 EnglishName="Sumo Dumbbell Squat",                PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="لانج راه‌رونده هالتر",            EnglishName="Barbell Walking Lunge",              PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="اسکات یک‌پا (پیستول اسکات)",      EnglishName="Pistol Squat",                       PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="پرس پا تک‌پا",                    EnglishName="Single-Leg Leg Press",               PrimaryMuscleGroupId=MG("چهارسر ران"),       EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("متوسط"),    IsActive=true },

            // ══════════════════════════════════════════
            // همسترینگ – Hamstrings
            // ══════════════════════════════════════════
            new() { Name="کرل پا دستگاه خوابیده",           EnglishName="Lying Leg Curl",                     PrimaryMuscleGroupId=MG("همسترینگ"),         EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="کرل پا دستگاه نشسته",             EnglishName="Seated Leg Curl",                    PrimaryMuscleGroupId=MG("همسترینگ"),         EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="ددلیفت رومانیایی دمبل",           EnglishName="Dumbbell Romanian Deadlift",         PrimaryMuscleGroupId=MG("همسترینگ"),         EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="گلوت‌هم ریز",                     EnglishName="Glute-Ham Raise",                    PrimaryMuscleGroupId=MG("همسترینگ"),         EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="نوردیک کرل",                      EnglishName="Nordic Hamstring Curl",              PrimaryMuscleGroupId=MG("همسترینگ"),         EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="کرل پا کابل",                     EnglishName="Cable Leg Curl",                     PrimaryMuscleGroupId=MG("همسترینگ"),         EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="ددلیفت تک‌پا دمبل",               EnglishName="Single-Leg Dumbbell RDL",            PrimaryMuscleGroupId=MG("همسترینگ"),         EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="هیپ هینج کتل‌بل",                 EnglishName="Kettlebell Hip Hinge",               PrimaryMuscleGroupId=MG("همسترینگ"),         EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="ددلیفت رومانیایی کابل",           EnglishName="Cable Romanian Deadlift",            PrimaryMuscleGroupId=MG("همسترینگ"),         EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },

            // ══════════════════════════════════════════
            // باسن – Glutes
            // ══════════════════════════════════════════
            new() { Name="هیپ تراست هالتر",                 EnglishName="Barbell Hip Thrust",                 PrimaryMuscleGroupId=MG("باسن (سرینی)"),     EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="هیپ تراست وزن بدن",               EnglishName="Bodyweight Hip Thrust",              PrimaryMuscleGroupId=MG("باسن (سرینی)"),     EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="گلوت بریج",                       EnglishName="Glute Bridge",                       PrimaryMuscleGroupId=MG("باسن (سرینی)"),     EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="کیک‌بک باسن کابل",                EnglishName="Cable Glute Kickback",               PrimaryMuscleGroupId=MG("باسن (سرینی)"),     EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="ابداکشن باسن دستگاه",             EnglishName="Hip Abduction Machine",              PrimaryMuscleGroupId=MG("باسن (سرینی)"),     EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="ددلیفت ساق صاف",                  EnglishName="Stiff-Leg Deadlift",                 PrimaryMuscleGroupId=MG("باسن (سرینی)"),     EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="فایر هایدرانت کش",                EnglishName="Band Fire Hydrant",                  PrimaryMuscleGroupId=MG("باسن (سرینی)"),     EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="سومو ددلیفت دمبل",                EnglishName="Sumo Dumbbell Deadlift",             PrimaryMuscleGroupId=MG("باسن (سرینی)"),     EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="هیپ تراست یک‌پا",                 EnglishName="Single-Leg Hip Thrust",              PrimaryMuscleGroupId=MG("باسن (سرینی)"),     EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="کلم‌شل کش",                       EnglishName="Clamshell with Band",                PrimaryMuscleGroupId=MG("باسن (سرینی)"),     EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),    IsActive=true },

            // ══════════════════════════════════════════
            // ساق پا – Calves
            // ══════════════════════════════════════════
            new() { Name="کرانچ ساق ایستاده دستگاه",        EnglishName="Standing Calf Raise (Machine)",      PrimaryMuscleGroupId=MG("ساق پا"),           EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="کرانچ ساق نشسته دستگاه",          EnglishName="Seated Calf Raise",                  PrimaryMuscleGroupId=MG("ساق پا"),           EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="کرانچ ساق پرس پا",                EnglishName="Leg Press Calf Raise",               PrimaryMuscleGroupId=MG("ساق پا"),           EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="کرانچ ساق وزن بدن",               EnglishName="Bodyweight Calf Raise",              PrimaryMuscleGroupId=MG("ساق پا"),           EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="کرانچ ساق یک‌پا",                 EnglishName="Single-Leg Calf Raise",              PrimaryMuscleGroupId=MG("ساق پا"),           EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="کرانچ ساق هالتر",                 EnglishName="Barbell Calf Raise",                 PrimaryMuscleGroupId=MG("ساق پا"),           EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="کرانچ ساق دمبل",                  EnglishName="Dumbbell Calf Raise",                PrimaryMuscleGroupId=MG("ساق پا"),           EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="جامپ رپ ساق",                     EnglishName="Jump Rope Calf Raise",               PrimaryMuscleGroupId=MG("ساق پا"),           EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },

            // ══════════════════════════════════════════
            // شکم و میان‌تنه – Core / Abs
            // ══════════════════════════════════════════
            new() { Name="کرانچ",                            EnglishName="Crunch",                             PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="کرانچ معکوس",                     EnglishName="Reverse Crunch",                     PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پلانک",                            EnglishName="Plank",                              PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="پلانک جانبی",                     EnglishName="Side Plank",                         PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="درازنشست",                         EnglishName="Sit-Up",                             PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="لگ ریز",                           EnglishName="Leg Raise",                          PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="هنگینگ لگ ریز",                   EnglishName="Hanging Leg Raise",                  PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="ماوتین کلایمبر",                  EnglishName="Mountain Climber",                   PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="کرانچ کابل",                      EnglishName="Cable Crunch",                       PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="توییست روسی",                     EnglishName="Russian Twist",                      PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="ویل رول‌اوت",                     EnglishName="Ab Wheel Rollout",                   PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="وود‌چاپ کابل",                    EnglishName="Cable Woodchop",                     PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="توئیست هالتر",                    EnglishName="Landmine Rotation",                  PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="ال‌سیت",                          EnglishName="L-Sit",                              PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="پلانک دست راست-چپ",               EnglishName="Alternating Arm Plank",              PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="هنگینگ نی ریز",                   EnglishName="Hanging Knee Raise",                 PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="دراگون فلگ",                      EnglishName="Dragon Flag",                        PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="بیرد راو پلانک",                  EnglishName="Bird-Dog",                           PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="توییست با وزنه",                  EnglishName="Weighted Russian Twist",             PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="V آپ",                             EnglishName="V-Up",                               PrimaryMuscleGroupId=MG("شکم و میان‌تنه"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),    IsActive=true },

            // ══════════════════════════════════════════
            // ساعد و مچ – Forearms & Wrists
            // ══════════════════════════════════════════
            new() { Name="رایست مچ هالتر",                  EnglishName="Barbell Wrist Curl",                 PrimaryMuscleGroupId=MG("ساعد و مچ"),        EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="رایست مچ معکوس هالتر",            EnglishName="Reverse Barbell Wrist Curl",         PrimaryMuscleGroupId=MG("ساعد و مچ"),        EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="رایست مچ دمبل",                   EnglishName="Dumbbell Wrist Curl",                PrimaryMuscleGroupId=MG("ساعد و مچ"),        EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="فارمر واک",                       EnglishName="Farmer's Walk",                      PrimaryMuscleGroupId=MG("ساعد و مچ"),        EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="هنگ گریپ",                        EnglishName="Dead Hang",                          PrimaryMuscleGroupId=MG("ساعد و مچ"),        EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="رادیال دویشن کابل",               EnglishName="Cable Radial Deviation",             PrimaryMuscleGroupId=MG("ساعد و مچ"),        EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },

            // ══════════════════════════════════════════
            // تمام بدن – Full Body
            // ══════════════════════════════════════════
            new() { Name="کلین اند پرس",                    EnglishName="Clean and Press",                    PrimaryMuscleGroupId=MG("تمام بدن"),         EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="کتل‌بل سوئینگ",                   EnglishName="Kettlebell Swing",                   PrimaryMuscleGroupId=MG("تمام بدن"),         EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="اسنچ هالتر",                      EnglishName="Barbell Snatch",                     PrimaryMuscleGroupId=MG("تمام بدن"),         EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="ترکیب اسکات و پرس",               EnglishName="Squat to Press",                     PrimaryMuscleGroupId=MG("تمام بدن"),         EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="بورپی",                            EnglishName="Burpee",                             PrimaryMuscleGroupId=MG("تمام بدن"),         EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="کتل‌بل ترکیبی (کلین)",            EnglishName="Kettlebell Clean",                   PrimaryMuscleGroupId=MG("تمام بدن"),         EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="باکس جامپ",                       EnglishName="Box Jump",                           PrimaryMuscleGroupId=MG("تمام بدن"),         EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="مدیسین بال اسلم",                 EnglishName="Medicine Ball Slam",                 PrimaryMuscleGroupId=MG("تمام بدن"),         EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },
            new() { Name="تراست کتل‌بل",                    EnglishName="Kettlebell Thruster",                PrimaryMuscleGroupId=MG("تمام بدن"),         EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("متوسط"),    IsActive=true },
            new() { Name="تراست هالتر",                     EnglishName="Barbell Thruster",                   PrimaryMuscleGroupId=MG("تمام بدن"),         EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="کلین هالتر",                      EnglishName="Power Clean",                        PrimaryMuscleGroupId=MG("تمام بدن"),         EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"),  IsActive=true },
            new() { Name="تی‌آر‌ایکس رو",                   EnglishName="TRX Row",                            PrimaryMuscleGroupId=MG("تمام بدن"),         EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("مبتدی"),    IsActive=true },




                        // ══════════════════════════════════════════
            // سینه – Chest (ادامه)
            // ══════════════════════════════════════════
            new() { Name="پرس سینه دمبل تک‌دست",            EnglishName="Single-Arm Dumbbell Bench Press",    PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="فلای دستگاه بالاسینه",            EnglishName="Incline Pec Deck Fly",               PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="شنا بالاسینه",                    EnglishName="Incline Push-Up",                    PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="شنا پایین‌سینه روی توپ",          EnglishName="Swiss Ball Push-Up",                 PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="شنا با دست باز",                  EnglishName="Wide-Grip Push-Up",                  PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پرس سینه کابل",                   EnglishName="Cable Chest Press",                  PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پرس سینه بالا دستگاه هامر",       EnglishName="Incline Hammer Chest Press",         PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="کراس‌اور کابل بالا",              EnglishName="High-to-Low Cable Crossover",        PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="کراس‌اور کابل پایین",             EnglishName="Low-to-High Cable Crossover",        PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پرس سینه هالتر معکوس",            EnglishName="Reverse Grip Bench Press",           PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="شنا انفجاری",                     EnglishName="Plyometric Push-Up",                 PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="پرس سینه پایین دستگاه",           EnglishName="Decline Machine Chest Press",        PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="فلای با کش تمرینی",               EnglishName="Band Chest Fly",                     PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پرس سینه با کش تمرینی",           EnglishName="Band Chest Press",                   PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="دیپس سینه پیشرفته",               EnglishName="Weighted Chest Dip",                 PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="پول‌اور هالتر",                   EnglishName="Barbell Pullover",                   PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="پرس دمبل بالاسینه تک‌دست",        EnglishName="Single-Arm Incline Dumbbell Press",  PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="فلای دمبل پایین‌سینه",            EnglishName="Decline Dumbbell Fly",               PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="شنا روی توپ سوئیسی",             EnglishName="Push-Up on Swiss Ball",              PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="پرس سینه با دمبل زاویه ۳۰",       EnglishName="30-Degree Dumbbell Bench Press",     PrimaryMuscleGroupId=MG("سینه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },

            // ══════════════════════════════════════════
            // پشت – Back (ادامه)
            // ══════════════════════════════════════════
            new() { Name="زیربغل هالتر معکوس",              EnglishName="Reverse Grip Barbell Row",           PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="زیربغل دمبل روی نیمکت",           EnglishName="Chest-Supported Dumbbell Row",       PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="زیربغل دستگاه نشسته دست باز",    EnglishName="Wide-Grip Seated Cable Row",          PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="اسنچ گریپ ددلیفت",                EnglishName="Snatch-Grip Deadlift",               PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="رک پول",                          EnglishName="Rack Pull",                          PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="بارفیکس وزنه‌دار",                EnglishName="Weighted Pull-Up",                   PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="لت پول‌داون تک‌دست کابل",         EnglishName="Single-Arm Lat Pulldown",            PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="رو کابل ایستاده",                 EnglishName="Standing Cable Row",                 PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="زیربغل کتل‌بل",                   EnglishName="Kettlebell Row",                     PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="ددلیفت تراپ‌بار",                 EnglishName="Trap Bar Deadlift",                  PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="زیربغل هالتر کندرا",              EnglishName="Pendlay Row",                        PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="لندماین رو",                      EnglishName="Landmine Row",                       PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="بارفیکس با کش کمکی",              EnglishName="Band-Assisted Pull-Up",              PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="ددلیفت ترکیبی",                   EnglishName="Deficit Deadlift",                   PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="هایپراکستنشن وزنه‌دار",           EnglishName="Weighted Back Extension",            PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="زیربغل دستگاه تک‌دست",            EnglishName="Single-Arm Machine Row",             PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="رو با دمبل روی نیمکت شیب‌دار",    EnglishName="Incline Bench Dumbbell Row",         PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="بارفیکس نزدیک‌دست",               EnglishName="Close-Grip Pull-Up",                 PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="رو با توپ TRX",                   EnglishName="TRX Suspension Row",                 PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="لت پول‌داون معکوس دست جمع",       EnglishName="Supinated Close-Grip Pulldown",      PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="ددلیفت بلوکی",                    EnglishName="Block Pull Deadlift",                PrimaryMuscleGroupId=MG("پشت"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"), IsActive=true },

            // ══════════════════════════════════════════
            // شانه – Shoulders (ادامه)
            // ══════════════════════════════════════════
            new() { Name="پرس سرشانه هالتر نشسته",          EnglishName="Seated Barbell Overhead Press",      PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="نشر جانبی کابل تک‌دست",           EnglishName="Single-Arm Cable Lateral Raise",     PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="نشر جلو کابل",                    EnglishName="Cable Front Raise",                  PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پرس سرشانه تک‌دست دمبل",          EnglishName="Single-Arm Dumbbell Press",          PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="پرس سرشانه با دو دمبل خم",        EnglishName="Bradford Press",                     PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="نشر خم روی نیمکت",                EnglishName="Prone Rear Delt Raise",              PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="کوبان روتیشن",                    EnglishName="Cuban Rotation",                     PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="فیس‌پول با کش",                   EnglishName="Band Face Pull",                     PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="ردفای دمبل",                      EnglishName="Dumbbell Upright Row",               PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پرس سرشانه دمبل ایستاده",         EnglishName="Standing Dumbbell Overhead Press",   PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="شراگ کابل",                       EnglishName="Cable Shrug",                        PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="شراگ دستگاه",                     EnglishName="Machine Shrug",                      PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="نشر جانبی با تقلب",               EnglishName="Cheat Lateral Raise",                PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="شانه بالاانداختن کتل‌بل",         EnglishName="Kettlebell High Pull",               PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="نشر جلو کش تمرینی",               EnglishName="Band Front Raise",                   PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="نشر خم کش تمرینی",                EnglishName="Band Rear Delt Fly",                 PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="یک‌طرفه آرنولد پرس",              EnglishName="Single-Arm Arnold Press",            PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="پرس هالتر زاویه ۳۰",              EnglishName="30-Degree Barbell Press",            PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="نشر جانبی دمبل لانج",             EnglishName="Lunge Lateral Raise",                PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="شراگ با کش تمرینی",               EnglishName="Band Shrug",                         PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پوش پرس دمبل",                    EnglishName="Dumbbell Push Press",                PrimaryMuscleGroupId=MG("شانه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },

            // ══════════════════════════════════════════
            // دوسر بازو – Biceps (ادامه)
            // ══════════════════════════════════════════
            new() { Name="جلوبازو اینکلاین دمبل",           EnglishName="Incline Dumbbell Curl",              PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="جلوبازو کش تمرینی ایستاده",       EnglishName="Standing Band Curl",                 PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="جلوبازو لاری کابل",               EnglishName="Cable Preacher Curl",                PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="جلوبازو چکشی روی نیمکت",          EnglishName="Incline Hammer Curl",                PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="جلوبازو متناوب دمبل",              EnglishName="Alternating Dumbbell Curl",          PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="جلوبازو زوتمن",                   EnglishName="Zottman Curl",                       PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="جلوبازو دیواری",                  EnglishName="Wall Curl",                          PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="جلوبازو کتل‌بل",                  EnglishName="Kettlebell Curl",                    PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="جلوبازو معکوس دمبل",              EnglishName="Reverse Dumbbell Curl",              PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="جلوبازو تمرکزی کابل",             EnglishName="Cable Concentration Curl",           PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="جلوبازو بالای سر کابل",           EnglishName="Overhead Cable Curl",                PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="جلوبازو معکوس EZ",                EnglishName="Reverse EZ-Bar Curl",                PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="جلوبازو دمبل روی میز اسکات",      EnglishName="Scott Bench Dumbbell Curl",          PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="جلوبازو تقلبی هالتر",             EnglishName="Cheat Barbell Curl",                 PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="جلوبازو دو کابل همزمان",          EnglishName="Double Cable Curl",                  PrimaryMuscleGroupId=MG("دوسر بازو"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },

            // ══════════════════════════════════════════
            // سه‌سر بازو – Triceps (ادامه)
            // ══════════════════════════════════════════
            new() { Name="اسکال‌کراشر دمبل",                EnglishName="Dumbbell Skull Crusher",             PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="پشت بازو کابل بالاسر",            EnglishName="Overhead Cable Tricep Extension",    PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="دیپس وزنه‌دار",                   EnglishName="Weighted Tricep Dip",                PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="پشت بازو تک‌دست دمبل ایستاده",    EnglishName="Standing Single-Arm DB Extension",   PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پشت بازو EZ بالاسر",              EnglishName="EZ-Bar Overhead Extension",          PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="پشت بازو دستگاه بالاسر",          EnglishName="Machine Overhead Tricep Extension",  PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پشت بازو کش تمرینی بالاسر",       EnglishName="Band Overhead Tricep Extension",     PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="کیک‌بک کابل",                     EnglishName="Cable Tricep Kickback",              PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="اسکال‌کراشر کابل",                EnglishName="Cable Skull Crusher",                PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="پشت بازو با دمبل دو‌طرفه",        EnglishName="Two-Arm Overhead Dumbbell Extension",PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="شنا برعکس ضربدری",                EnglishName="Diamond Push-Up (Tricep Focus)",     PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="پشت بازو روی توپ",                EnglishName="Swiss Ball Tricep Extension",        PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="JM پرس",                          EnglishName="JM Press",                           PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="پشت بازو پارالل بار",             EnglishName="Parallel Bar Tricep Dip",            PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="لندماین پرس پشت بازو",            EnglishName="Landmine Tricep Press",              PrimaryMuscleGroupId=MG("سه‌سر بازو"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },

            // ══════════════════════════════════════════
            // چهارسر ران – Quadriceps (ادامه)
            // ══════════════════════════════════════════
            new() { Name="اسکات اسمیت",                     EnglishName="Smith Machine Squat",                PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="اسکات جامپ",                      EnglishName="Jump Squat",                         PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="اسکات اوورهد",                    EnglishName="Overhead Squat",                     PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="لانج جانبی دمبل",                 EnglishName="Lateral Dumbbell Lunge",             PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="اسکات زرچر",                      EnglishName="Zercher Squat",                      PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="اسکات اسپانیایی با کش",           EnglishName="Spanish Squat (Band)",               PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="لانج اسمیت",                      EnglishName="Smith Machine Lunge",                PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="استپ‌آپ هالتر",                   EnglishName="Barbell Step-Up",                    PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="اکستنشن پا تک‌پا دستگاه",         EnglishName="Single-Leg Leg Extension",           PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="اسکات پرده",                      EnglishName="Curtsy Squat",                       PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پرس پا ۴۵ درجه",                  EnglishName="45-Degree Leg Press",                PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="لانج معکوس هالتر",                EnglishName="Barbell Reverse Lunge",              PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="اسکات باند",                      EnglishName="Banded Squat",                       PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="لانج ۳۶۰",                        EnglishName="360 Lunge",                          PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="کوزاک اسکات",                     EnglishName="Cossack Squat",                      PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="اسکات پا جمع",                    EnglishName="Narrow Stance Squat",                PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="لگ پرس تک‌پا ۴۵ درجه",            EnglishName="Single-Leg 45-Degree Leg Press",     PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="اسکات با توپ دیواری",             EnglishName="Wall Ball Squat",                    PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="اسکات پیش‌گام",                   EnglishName="Anderson Squat",                     PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="نیمه‌اسکات هالتر",                EnglishName="Half Squat",                         PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("مبتدی"),   IsActive=true },

            // ══════════════════════════════════════════
            // همسترینگ – Hamstrings (ادامه)
            // ══════════════════════════════════════════
            new() { Name="کرل پا ایستاده دستگاه",           EnglishName="Standing Leg Curl",                  PrimaryMuscleGroupId=MG("همسترینگ"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="ددلیفت رومانیایی هالتر",          EnglishName="Barbell Romanian Deadlift",          PrimaryMuscleGroupId=MG("همسترینگ"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="کرل پا کش تمرینی",                EnglishName="Band Leg Curl",                      PrimaryMuscleGroupId=MG("همسترینگ"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="گود مورنینگ هالتر",               EnglishName="Good Morning (Barbell)",             PrimaryMuscleGroupId=MG("همسترینگ"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="ددلیفت رومانیایی تک‌پا هالتر",    EnglishName="Single-Leg Barbell RDL",             PrimaryMuscleGroupId=MG("همسترینگ"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="اینچ ورم",                        EnglishName="Inchworm",                           PrimaryMuscleGroupId=MG("همسترینگ"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="سوئینگ کتل‌بل",                   EnglishName="Kettlebell Swing (Hamstring Focus)",  PrimaryMuscleGroupId=MG("همسترینگ"), EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="کرل پا توپ سوئیسی",               EnglishName="Swiss Ball Leg Curl",                PrimaryMuscleGroupId=MG("همسترینگ"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="ددلیفت رومانیایی اسمیت",          EnglishName="Smith Machine RDL",                  PrimaryMuscleGroupId=MG("همسترینگ"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="هیپ هینج وزن بدن",                EnglishName="Bodyweight Hip Hinge",               PrimaryMuscleGroupId=MG("همسترینگ"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="ددلیفت رومانیایی کتل‌بل",         EnglishName="Kettlebell Romanian Deadlift",       PrimaryMuscleGroupId=MG("همسترینگ"), EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },

            // ══════════════════════════════════════════
            // باسن – Glutes (ادامه)
            // ══════════════════════════════════════════
            new() { Name="هیپ تراست دستگاه",                EnglishName="Machine Hip Thrust",                 PrimaryMuscleGroupId=MG("باسن (سرینی)"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="گلوت بریج وزنه‌دار",              EnglishName="Weighted Glute Bridge",              PrimaryMuscleGroupId=MG("باسن (سرینی)"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="کیک‌بک باسن کش",                  EnglishName="Band Glute Kickback",                PrimaryMuscleGroupId=MG("باسن (سرینی)"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پاشنه‌راست کش",                   EnglishName="Band Standing Kickback",             PrimaryMuscleGroupId=MG("باسن (سرینی)"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="ادداکشن ران دستگاه",              EnglishName="Hip Adduction Machine",              PrimaryMuscleGroupId=MG("باسن (سرینی)"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="کتل‌بل هیپ تراست",                EnglishName="Kettlebell Hip Thrust",              PrimaryMuscleGroupId=MG("باسن (سرینی)"), EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="دانکی کیک کش",                    EnglishName="Donkey Kick with Band",              PrimaryMuscleGroupId=MG("باسن (سرینی)"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="ددلیفت پا جمع رومانیایی",         EnglishName="Narrow Stance Romanian Deadlift",    PrimaryMuscleGroupId=MG("باسن (سرینی)"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="اسکات سومو هالتر",                EnglishName="Barbell Sumo Squat",                 PrimaryMuscleGroupId=MG("باسن (سرینی)"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="ابداکشن کابل ایستاده",            EnglishName="Standing Cable Hip Abduction",       PrimaryMuscleGroupId=MG("باسن (سرینی)"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="گلوت بریج تک‌پا وزنه‌دار",        EnglishName="Weighted Single-Leg Glute Bridge",   PrimaryMuscleGroupId=MG("باسن (سرینی)"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="پل باسن با توپ",                  EnglishName="Swiss Ball Glute Bridge",            PrimaryMuscleGroupId=MG("باسن (سرینی)"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },

            // ══════════════════════════════════════════
            // ساق پا – Calves (ادامه)
            // ══════════════════════════════════════════
            new() { Name="کرانچ ساق دستگاه ایستاده تک‌پا",  EnglishName="Single-Leg Standing Calf Raise Machine",PrimaryMuscleGroupId=MG("ساق پا"), EquipmentTypeId=EQ("دستگاه"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="کرانچ ساق با کش",                 EnglishName="Band Calf Raise",                    PrimaryMuscleGroupId=MG("ساق پا"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="کرانچ ساق روی پله",               EnglishName="Step Calf Raise",                    PrimaryMuscleGroupId=MG("ساق پا"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="کرانچ ساق کتل‌بل",                EnglishName="Kettlebell Calf Raise",              PrimaryMuscleGroupId=MG("ساق پا"), EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="کرانچ ساق وزنه روی پله یک‌پا",    EnglishName="Weighted Single-Leg Step Calf Raise",PrimaryMuscleGroupId=MG("ساق پا"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="اسکیپینگ ساق",                    EnglishName="Skipping Calf Drill",                PrimaryMuscleGroupId=MG("ساق پا"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="کرانچ ساق هالتر ایستاده تک‌پا",   EnglishName="Single-Leg Barbell Calf Raise",      PrimaryMuscleGroupId=MG("ساق پا"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },

            // ══════════════════════════════════════════
            // شکم و میان‌تنه – Core / Abs (ادامه)
            // ══════════════════════════════════════════
            new() { Name="کرانچ با توپ",                    EnglishName="Swiss Ball Crunch",                  PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="توییست با توپ",                   EnglishName="Swiss Ball Russian Twist",           PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="پایک رول‌اوت",                    EnglishName="Swiss Ball Pike Rollout",            PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="کرانچ معکوس روی نیمکت شیب‌دار",   EnglishName="Decline Reverse Crunch",             PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="وودچاپ دمبل",                     EnglishName="Dumbbell Woodchop",                  PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="هالو باتل رینگ",                  EnglishName="Hollow Body Hold",                   PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="توییست پا دوچرخه",                EnglishName="Bicycle Crunch",                     PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="رول‌اوت با هالتر",                EnglishName="Barbell Rollout",                    PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="کابل کرانچ نشسته",                EnglishName="Seated Cable Crunch",                PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="تویست لندماین",                   EnglishName="Landmine 180",                       PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="پلانک با دست",                    EnglishName="Straight-Arm Plank",                 PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پلانک جانبی با حرکت",             EnglishName="Moving Side Plank",                  PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="هنگینگ ونیتی",                    EnglishName="Toes-to-Bar",                        PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="رانینگ ماون",                     EnglishName="Running Man (Core)",                 PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پاندول پا",                       EnglishName="Leg Pendulum",                       PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="اسپایدرمن پلانک",                 EnglishName="Spiderman Plank",                    PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="ددباگ",                           EnglishName="Dead Bug",                           PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="هیپ رول",                         EnglishName="Hip Roll",                           PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="کرانچ با وزنه دست بالا",          EnglishName="Overhead Weighted Crunch",           PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="فلاتر کیک",                       EnglishName="Flutter Kick",                       PrimaryMuscleGroupId=MG("شکم و میان‌تنه"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },

            // ══════════════════════════════════════════
            // ساعد و مچ – Forearms (ادامه)
            // ══════════════════════════════════════════
            new() { Name="پینچ گریپ دمبل",                  EnglishName="Pinch Grip Dumbbell Hold",           PrimaryMuscleGroupId=MG("ساعد و مچ"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="رایست مچ کابل",                   EnglishName="Cable Wrist Curl",                   PrimaryMuscleGroupId=MG("ساعد و مچ"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="فارمر واک کتل‌بل",                EnglishName="Kettlebell Farmer's Walk",           PrimaryMuscleGroupId=MG("ساعد و مچ"), EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="رول مچ با وزنه",                  EnglishName="Wrist Roller",                       PrimaryMuscleGroupId=MG("ساعد و مچ"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="نگه‌داشتن بشقاب",                 EnglishName="Plate Pinch",                        PrimaryMuscleGroupId=MG("ساعد و مچ"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="گریپ کراشر",                      EnglishName="Grip Crush (Hand Gripper)",          PrimaryMuscleGroupId=MG("ساعد و مچ"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="لاگ پرس",                         EnglishName="Log Press (Strongman)",              PrimaryMuscleGroupId=MG("ساعد و مچ"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("پیشرفته"), IsActive=true },

            // ══════════════════════════════════════════
            // تمام بدن – Full Body (ادامه)
            // ══════════════════════════════════════════
            new() { Name="تور کتل‌بل",                      EnglishName="Kettlebell Turkish Get-Up",          PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="مانستر واک",                      EnglishName="Monster Walk (Band)",                PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="اسکات تراست",                     EnglishName="Squat Thruster",                     PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="جامپینگ جک",                      EnglishName="Jumping Jack",                       PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="اسپرینت جا",                      EnglishName="High Knees Sprint",                  PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="مانع پرش",                        EnglishName="Hurdle Jump",                        PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="پاور کلین از زمین",               EnglishName="Power Clean from Floor",             PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="بال اسلم",                        EnglishName="Battle Rope Slam",                   PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="رپ جامپ",                         EnglishName="Jump Rope",                          PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="سنباگ کری",                       EnglishName="Sandbag Carry",                      PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="رپ باتل رینگ",                    EnglishName="Battle Rope Waves",                  PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="کتل‌بل فلور پرس",                 EnglishName="Kettlebell Floor Press",             PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="استرانگمن یوک واک",               EnglishName="Yoke Walk",                          PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="سنباگ اسکوات",                    EnglishName="Sandbag Squat",                      PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="تایر فلیپ",                       EnglishName="Tire Flip",                          PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="اسلد پوش",                        EnglishName="Sled Push",                          PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="اسلد پول",                        EnglishName="Sled Pull",                          PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="وال بال",                         EnglishName="Wall Ball",                          PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="دبل کتل‌بل کلین اند پرس",         EnglishName="Double Kettlebell Clean and Press",  PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="برپی باکس جامپ",                  EnglishName="Burpee Box Jump",                    PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("پیشرفته"), IsActive=true },

            // ══════════════════════════════════════════
            // کمر – Lower Back
            // ══════════════════════════════════════════
            new() { Name="گود مورنینگ دمبل",                EnglishName="Dumbbell Good Morning",              PrimaryMuscleGroupId=MG("کمر"),  EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="هایپراکستنشن معکوس",              EnglishName="Reverse Hyperextension",             PrimaryMuscleGroupId=MG("کمر"),  EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="ددلیفت تک‌پا هالتر",              EnglishName="Single-Leg Barbell Deadlift",        PrimaryMuscleGroupId=MG("کمر"),  EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("پیشرفته"), IsActive=true },
            new() { Name="هایپراکستنشن ۴۵ درجه",            EnglishName="45-Degree Back Extension",           PrimaryMuscleGroupId=MG("کمر"),  EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="سوپرمن",                          EnglishName="Superman",                           PrimaryMuscleGroupId=MG("کمر"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="بریج پشت",                        EnglishName="Back Bridge",                        PrimaryMuscleGroupId=MG("کمر"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="گود مورنینگ نشسته",               EnglishName="Seated Good Morning",                PrimaryMuscleGroupId=MG("کمر"),  EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="کوادروپد آرم/لگ",                 EnglishName="Quadruped Arm/Leg Raise",            PrimaryMuscleGroupId=MG("کمر"),  EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="هیپ اکستنشن کابل",                EnglishName="Cable Hip Extension",                PrimaryMuscleGroupId=MG("کمر"),  EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="ددلیفت دستگاه",                   EnglishName="Machine Deadlift",                   PrimaryMuscleGroupId=MG("کمر"),  EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },

            // ══════════════════════════════════════════
            // ران داخلی – Inner Thigh (Adductors)
            // ══════════════════════════════════════════
            new() { Name="ادداکشن کابل",                    EnglishName="Cable Hip Adduction",                PrimaryMuscleGroupId=MG("ران داخلی"), EquipmentTypeId=EQ("کابل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="ادداکشن دستگاه نشسته",            EnglishName="Seated Hip Adduction Machine",       PrimaryMuscleGroupId=MG("ران داخلی"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="لانج جانبی",                      EnglishName="Lateral Lunge",                      PrimaryMuscleGroupId=MG("ران داخلی"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="لانج جانبی دمبل هالتر",           EnglishName="Dumbbell Lateral Lunge",             PrimaryMuscleGroupId=MG("ران داخلی"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="اسکات سومو با کش",                EnglishName="Banded Sumo Squat",                  PrimaryMuscleGroupId=MG("ران داخلی"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="باله پلیه اسکات",                 EnglishName="Ballet Plié Squat",                  PrimaryMuscleGroupId=MG("ران داخلی"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="ادداکشن با توپ",                  EnglishName="Ball Squeeze Adduction",             PrimaryMuscleGroupId=MG("ران داخلی"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },

            // ══════════════════════════════════════════
            // ران خارجی – Outer Thigh (Abductors)
            // ══════════════════════════════════════════
            new() { Name="ابداکشن دستگاه ایستاده",          EnglishName="Standing Hip Abduction Machine",     PrimaryMuscleGroupId=MG("ران خارجی"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="ابداکشن کش خوابیده",              EnglishName="Lying Band Hip Abduction",           PrimaryMuscleGroupId=MG("ران خارجی"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="ابداکشن کش ایستاده",              EnglishName="Standing Band Hip Abduction",        PrimaryMuscleGroupId=MG("ران خارجی"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پهلو گام کش",                     EnglishName="Band Side Step",                     PrimaryMuscleGroupId=MG("ران خارجی"), EquipmentTypeId=EQ("کش تمرینی"),     DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="کلم‌شل ایستاده",                  EnglishName="Standing Clamshell",                 PrimaryMuscleGroupId=MG("ران خارجی"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("مبتدی"),   IsActive=true },

            // ══════════════════════════════════════════
            // سینه‌ریزه (گردن و تله‌پزیوس فوقانی) – Traps & Neck
            // ══════════════════════════════════════════
            new() { Name="شراگ پشت هالتر",                  EnglishName="Behind-Back Barbell Shrug",          PrimaryMuscleGroupId=MG("تله‌پزیوس"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="شراگ یک‌طرفه دمبل",               EnglishName="Single-Arm Dumbbell Shrug",          PrimaryMuscleGroupId=MG("تله‌پزیوس"), EquipmentTypeId=EQ("دمبل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="شراگ کتل‌بل",                     EnglishName="Kettlebell Shrug",                   PrimaryMuscleGroupId=MG("تله‌پزیوس"), EquipmentTypeId=EQ("کتل‌بل"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="شراگ وزنه اوتراپ",                EnglishName="Trap Bar Shrug",                     PrimaryMuscleGroupId=MG("تله‌پزیوس"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="پاور شراگ",                       EnglishName="Power Shrug",                        PrimaryMuscleGroupId=MG("تله‌پزیوس"), EquipmentTypeId=EQ("هالتر (باربل)"), DifficultyLevelId=DL("متوسط"),   IsActive=true },

            // ══════════════════════════════════════════
            // کاردیو و چابکی – Cardio & Agility
            // ══════════════════════════════════════════
            new() { Name="دوی سرعت",                        EnglishName="Sprint",                             PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="اسکی ارگ",                        EnglishName="Ski Erg",                            PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="رویینگ ارگ",                      EnglishName="Rowing Ergometer",                   PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="دوی استقامت تردمیل",              EnglishName="Treadmill Run",                      PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="دوچرخه ثابت",                     EnglishName="Stationary Bike",                    PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="الیپتیکال",                       EnglishName="Elliptical",                         PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="استپر",                           EnglishName="Stair Climber",                      PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("دستگاه"),        DifficultyLevelId=DL("مبتدی"),   IsActive=true },
            new() { Name="دانکی جامپ",                      EnglishName="Donkey Kick Jump",                   PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="اسکیتر",                          EnglishName="Skater Jump",                        PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="سرعت نردبان چابکی",               EnglishName="Agility Ladder Drill",               PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("سایر"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="T-Test دو",                       EnglishName="T-Test Agility Run",                 PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),   IsActive=true },
            new() { Name="پلایومتریک لانج",                 EnglishName="Plyometric Lunge",                   PrimaryMuscleGroupId=MG("تمام بدن"), EquipmentTypeId=EQ("وزن بدن"),       DifficultyLevelId=DL("متوسط"),   IsActive=true },


            // ==================== سینه (Chest) ====================
new() { Name="پرس سینه با هالتر",               EnglishName="Barbell Bench Press",                PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرس سینه با دمبل",                EnglishName="Dumbbell Bench Press",               PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پرس سینه شیب‌دار با هالتر",       EnglishName="Incline Barbell Bench Press",         PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرس سینه شیب‌دار با دمبل",        EnglishName="Incline Dumbbell Bench Press",        PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرس سینه سربالا با دمبل",         EnglishName="Decline Dumbbell Bench Press",        PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="فلای سینه با دمبل",               EnglishName="Dumbbell Fly",                        PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="فلای شیب‌دار با دمبل",            EnglishName="Incline Dumbbell Fly",                PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="کراس‌اوور با کابل",               EnglishName="Cable Crossover",                     PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پشو با دست باز",                  EnglishName="Wide Push-Up",                        PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پشو الماسی",                      EnglishName="Diamond Push-Up",                     PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پشو انفجاری",                     EnglishName="Explosive Push-Up",                   PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="پشو با دست تنگ",                  EnglishName="Close-Grip Push-Up",                  PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرس سینه دستگاه",                 EnglishName="Machine Chest Press",                 PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پرس سینه با کتل‌بل",              EnglishName="Kettlebell Chest Press",              PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="دیپ سینه",                        EnglishName="Chest Dip",                           PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== پشت (Back) ====================
new() { Name="ددلیفت رومانیایی",                EnglishName="Romanian Deadlift",                   PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="زیربغل هالتر خم",                 EnglishName="Bent-Over Barbell Row",               PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="زیربغل دمبل تک دست",              EnglishName="Single-Arm Dumbbell Row",             PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="زیربغل کابل نشسته",               EnglishName="Seated Cable Row",                    PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="بارفیکس دست کشیده",               EnglishName="Wide-Grip Pull-Up",                   PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="بارفیکس دست تنگ",                 EnglishName="Close-Grip Pull-Up",                  PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="بارفیکس معکوس",                   EnglishName="Chin-Up",                             PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="لت پول‌داون",                     EnglishName="Lat Pulldown",                        PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="لت پول‌داون دست تنگ",             EnglishName="Close-Grip Lat Pulldown",             PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="شراگ هالتر",                      EnglishName="Barbell Shrug",                       PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="شراگ دمبل",                       EnglishName="Dumbbell Shrug",                      PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="هایپراکستنشن",                    EnglishName="Hyperextension",                      PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="زیربغل تی‌بار",                   EnglishName="T-Bar Row",                           PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ددلیفت سوموئو",                   EnglishName="Sumo Deadlift",                       PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="کشش کابل بالا",                   EnglishName="High Cable Row",                      PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== سرشانه (Shoulders) ====================
new() { Name="پرس سرشانه با هالتر",             EnglishName="Barbell Overhead Press",              PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرس سرشانه با دمبل",              EnglishName="Dumbbell Shoulder Press",             PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="نشر جانب با دمبل",                EnglishName="Lateral Raise",                       PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="نشر جلو با دمبل",                 EnglishName="Front Raise",                         PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="نشر خم با دمبل",                  EnglishName="Bent-Over Lateral Raise",             PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="آرنولد پرس",                      EnglishName="Arnold Press",                        PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="نشر جانب با کابل",                EnglishName="Cable Lateral Raise",                 PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پرس سرشانه دستگاه اسمیت",        EnglishName="Smith Machine Shoulder Press",        PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اپرایت رو هالتر",                 EnglishName="Barbell Upright Row",                 PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="فیس‌پول با کابل",                 EnglishName="Face Pull",                           PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پشو شانه",                        EnglishName="Pike Push-Up",                        PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="هندستند پشو",                     EnglishName="Handstand Push-Up",                   PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },

// ==================== جلو بازو (Biceps) ====================
new() { Name="جلو بازو هالتر ایستاده",          EnglishName="Standing Barbell Curl",               PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو دمبل ایستاده",           EnglishName="Standing Dumbbell Curl",              PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو چکشی",                   EnglishName="Hammer Curl",                         PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو تمرکزی",                 EnglishName="Concentration Curl",                  PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو لاری",                   EnglishName="Preacher Curl",                       PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو کابل پایین",             EnglishName="Low Cable Curl",                      PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو رو میزی با دمبل",        EnglishName="Incline Dumbbell Curl",               PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="جلو بازو ریورس",                  EnglishName="Reverse Curl",                        PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو کابل تک دست",            EnglishName="Single-Arm Cable Curl",               PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو هالتر EZ",               EnglishName="EZ Bar Curl",                         PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو زیر دست با کابل",        EnglishName="Underhand Cable Curl",                PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== پشت بازو (Triceps) ====================
new() { Name="پشت بازو سیم‌کش بالا",            EnglishName="Cable Tricep Pushdown",               PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پشت بازو دیپ",                    EnglishName="Tricep Dip",                          PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اسکال‌کراشر",                     EnglishName="Skull Crusher",                       PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پشت بازو دمبل پشت سر",            EnglishName="Dumbbell Overhead Tricep Extension",  PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پشت بازو کابل طنابی",             EnglishName="Rope Tricep Pushdown",                PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پشت بازو تک دست با دمبل",         EnglishName="Single-Arm Dumbbell Kickback",        PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پرس پشت بازو هالتر تنگ",          EnglishName="Close-Grip Bench Press",              PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پشت بازو کابل یک‌دست",            EnglishName="Single-Arm Cable Pushdown",           PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پشت بازو کابل پشت سر",            EnglishName="Cable Overhead Tricep Extension",     PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== پا جلو (Quads) ====================
new() { Name="اسکوات هالتر",                    EnglishName="Barbell Back Squat",                  PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اسکوات جلو",                      EnglishName="Front Squat",                         PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="اسکوات بلغاری",                   EnglishName="Bulgarian Split Squat",               PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اسکوات تک پا",                    EnglishName="Single-Leg Squat",                    PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="اسکوات دستگاه",                   EnglishName="Hack Squat Machine",                  PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پرس پا",                          EnglishName="Leg Press",                           PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اکستنشن پا",                      EnglishName="Leg Extension",                       PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="لانج قدم رو",                     EnglishName="Walking Lunge",                       PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="لانج جانبی",                      EnglishName="Side Lunge",                          PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اسکوات سوموئو",                   EnglishName="Sumo Squat",                          PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ولت اسکوات",                      EnglishName="Wall Squat",                          PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اسکوات با کتل‌بل",               EnglishName="Kettlebell Goblet Squat",             PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پیستول اسکوات",                   EnglishName="Pistol Squat",                        PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },

// ==================== پا پشت (Hamstrings) ====================
new() { Name="لگ کرل دستگاه",                   EnglishName="Lying Leg Curl",                      PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="لگ کرل نشسته",                    EnglishName="Seated Leg Curl",                     PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کرل پا با دمبل",                  EnglishName="Dumbbell Leg Curl",                   PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ددلیفت با پاهای صاف",             EnglishName="Stiff-Leg Deadlift",                  PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="نوردیک کرل",                      EnglishName="Nordic Hamstring Curl",               PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="پل باسن تک پا",                   EnglishName="Single-Leg Glute Bridge",             PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کرل پا با کابل",                  EnglishName="Cable Leg Curl",                      PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اسکوات رومانیایی",                EnglishName="Romanian Squat",                      PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== سرینی (Glutes) ====================
new() { Name="هیپ تراست",                       EnglishName="Hip Thrust",                          PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="هیپ تراست با دمبل",               EnglishName="Dumbbell Hip Thrust",                 PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پل باسن",                         EnglishName="Glute Bridge",                        PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کیک‌بک باسن با کابل",             EnglishName="Cable Glute Kickback",                PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ددلیفت تک پا",                    EnglishName="Single-Leg Deadlift",                 PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="باند هیپ ابداکشن",                EnglishName="Banded Hip Abduction",                PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اسکوات پایه پهن",                 EnglishName="Wide-Stance Squat",                   PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="لانج معکوس",                      EnglishName="Reverse Lunge",                       PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="استپ آپ",                         EnglishName="Step Up",                             PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== ساق (Calves) ====================
new() { Name="کرل ساق ایستاده با دستگاه",       EnglishName="Standing Calf Raise Machine",         PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کرل ساق نشسته با دستگاه",         EnglishName="Seated Calf Raise Machine",           PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کرل ساق تک پا",                   EnglishName="Single-Leg Calf Raise",               PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کرل ساق با دمبل",                 EnglishName="Dumbbell Calf Raise",                 PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کرل ساق با هالتر",                EnglishName="Barbell Calf Raise",                  PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کرل ساق روی پله",                 EnglishName="Donkey Calf Raise",                   PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پرش طناب",                        EnglishName="Jump Rope",                           PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("طناب"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== هسته (Core) ====================
new() { Name="کرانچ",                           EnglishName="Crunch",                              PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کرانچ معکوس",                     EnglishName="Reverse Crunch",                      PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پلانک",                           EnglishName="Plank",                               PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پلانک جانبی",                     EnglishName="Side Plank",                          PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="چرخش روسی",                       EnglishName="Russian Twist",                       PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="بیرون دادن پا",                   EnglishName="Leg Raise",                           PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="چاقو جیبی",                       EnglishName="Jackknife",                           PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="کرانچ با کابل",                   EnglishName="Cable Crunch",                        PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="دوچرخه کرانچ",                    EnglishName="Bicycle Crunch",                      PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="توپ چرخشی",                       EnglishName="Ab Wheel Rollout",                    PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="V-آپ",                            EnglishName="V-Up",                                PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="بالا آوردن پا آویزان",             EnglishName="Hanging Leg Raise",                   PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="درازنشست",                        EnglishName="Sit-Up",                              PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="مرده متحرک",                      EnglishName="Dead Bug",                            PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پالوف پرس با کابل",               EnglishName="Pallof Press",                        PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== ساعد (Forearms) ====================
new() { Name="کرل مچ هالتر",                    EnglishName="Barbell Wrist Curl",                  PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کرل مچ معکوس",                    EnglishName="Reverse Wrist Curl",                  PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="چرخش مچ با دمبل",                 EnglishName="Dumbbell Wrist Rotation",             PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="فشار توپ مچ",                     EnglishName="Grip Ball Squeeze",                   PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اکستنشن مچ با دمبل",              EnglishName="Dumbbell Wrist Extension",             PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پیچش مچ با نوار",                 EnglishName="Wrist Roller",                        PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== تمام بدن (Full Body) ====================
new() { Name="کلین اند جرک",                    EnglishName="Clean and Jerk",                      PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="اسنچ",                            EnglishName="Snatch",                              PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="ترکیب بارپی",                     EnglishName="Burpee",                              PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ترکیب بارپی با پشو",              EnglishName="Burpee Push-Up",                      PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="کتل‌بل سوینگ",                    EnglishName="Kettlebell Swing",                    PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="کتل‌بل کلین",                     EnglishName="Kettlebell Clean",                    PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="کتل‌بل اسنچ",                     EnglishName="Kettlebell Snatch",                   PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="باکس جامپ",                       EnglishName="Box Jump",                            PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="تی آر ایکس رو",                   EnglishName="TRX Row",                             PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("TRX"),             DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="تی آر ایکس پشو",                  EnglishName="TRX Push-Up",                         PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("TRX"),             DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="تی آر ایکس اسکوات",               EnglishName="TRX Squat",                           PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("TRX"),             DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="مانع پرش",                        EnglishName="Hurdle Jump",                         PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="بیر کرال",                        EnglishName="Bear Crawl",                          PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="خرچنگ راه رفتن",                  EnglishName="Crab Walk",                           PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== کمر پایینی (Lower Back) ====================
new() { Name="گربه و گاو",                      EnglishName="Cat-Cow Stretch",                     PrimaryMuscleGroupId=MG("کمر پایینی"),EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="سوپرمن",                          EnglishName="Superman",                            PrimaryMuscleGroupId=MG("کمر پایینی"),EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="بریج",                            EnglishName="Bridge",                              PrimaryMuscleGroupId=MG("کمر پایینی"),EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="بردهاستر اکستنشن",                EnglishName="Back Extension",                      PrimaryMuscleGroupId=MG("کمر پایینی"),EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کوادروپد اکستنشن",                EnglishName="Bird Dog",                            PrimaryMuscleGroupId=MG("کمر پایینی"),EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ددلیفت تک پا رومانیایی",          EnglishName="Single-Leg Romanian Deadlift",        PrimaryMuscleGroupId=MG("کمر پایینی"),EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== کاردیو (Cardio) ====================
new() { Name="اسپرینت",                         EnglishName="Sprint",                              PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="دویدن با تردمیل",                 EnglishName="Treadmill Run",                       PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="دوچرخه ثابت",                     EnglishName="Stationary Bike",                     PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="الیپتیکال",                       EnglishName="Elliptical",                          PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="روئینگ با دستگاه",                EnglishName="Rowing Machine",                      PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پله نوردی",                       EnglishName="Stair Climber",                       PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اسکی‌اِر",                        EnglishName="Ski Erg",                             PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="جامپینگ جک",                      EnglishName="Jumping Jack",                        PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کوهنوردی",                        EnglishName="Mountain Climber",                    PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرش اسکوات",                      EnglishName="Jump Squat",                          PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="هاپ جانبی",                       EnglishName="Lateral Hop",                         PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== تله‌پز (Traps) ====================
new() { Name="شراگ با کابل",                    EnglishName="Cable Shrug",                         PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اپرایت رو با دمبل",               EnglishName="Dumbbell Upright Row",                PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="شراگ با کتل‌بل",                  EnglishName="Kettlebell Shrug",                    PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پاور کلین",                       EnglishName="Power Clean",                         PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="های‌پول",                         EnglishName="High Pull",                           PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== ادداکتور (Adductors) ====================
new() { Name="ادداکشن پا با دستگاه",            EnglishName="Machine Hip Adduction",               PrimaryMuscleGroupId=MG("ادداکتور"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ادداکشن با کابل",                 EnglishName="Cable Hip Adduction",                 PrimaryMuscleGroupId=MG("ادداکتور"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },

new() { Name="اسکوات سوموئو پهن",               EnglishName="Wide Sumo Squat",                     PrimaryMuscleGroupId=MG("ادداکتور"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== ابداکتور (Abductors) ====================
new() { Name="ابداکشن پا با دستگاه",            EnglishName="Machine Hip Abduction",               PrimaryMuscleGroupId=MG("ابداکتور"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ابداکشن با کابل",                 EnglishName="Cable Hip Abduction",                 PrimaryMuscleGroupId=MG("ابداکتور"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کلم‌شل",                          EnglishName="Clamshell",                           PrimaryMuscleGroupId=MG("ابداکتور"),   EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ابداکشن پا خوابیده",              EnglishName="Lying Hip Abduction",                 PrimaryMuscleGroupId=MG("ابداکتور"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="فایر هایدرنت",                    EnglishName="Fire Hydrant",                        PrimaryMuscleGroupId=MG("ابداکتور"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },






// ==================== سینه - تکمیلی ====================
new() { Name="پشو با دست یک طرف",               EnglishName="Archer Push-Up",                      PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="پشو پرشی با کف زدن",              EnglishName="Clap Push-Up",                        PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="پرس سینه تک دست با دمبل",         EnglishName="Single-Arm Dumbbell Press",           PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="فلای با کابل پایین",               EnglishName="Low Cable Fly",                       PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="فلای با کابل بالا",                EnglishName="High Cable Fly",                      PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرس سینه شیب معکوس با هالتر",     EnglishName="Decline Barbell Bench Press",         PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پشو با توپ طبی",                  EnglishName="Medicine Ball Push-Up",               PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("توپ طبی"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="فلای دستگاه پک‌دک",               EnglishName="Pec Deck Fly",                        PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پشو سوئیسی",                      EnglishName="Swiss Ball Push-Up",                  PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("توپ سوئیسی"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرس سینه با باند",                EnglishName="Resistance Band Chest Press",         PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پشو انگشتی",                      EnglishName="Fingertip Push-Up",                   PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="فلای با باند مقاومتی",            EnglishName="Resistance Band Fly",                 PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== پشت - تکمیلی ====================
new() { Name="زیربغل با باند مقاومتی",          EnglishName="Resistance Band Row",                 PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پول‌اور با دمبل",                 EnglishName="Dumbbell Pullover",                   PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پول‌اور با کابل",                 EnglishName="Cable Pullover",                      PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="مدموزیل پرس",                     EnglishName="Meadows Row",                         PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اینورتد رو",                      EnglishName="Inverted Row",                        PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="یک طرفه کابل رو",                 EnglishName="Single-Arm Cable Row",                PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="TRX زیربغل",                      EnglishName="TRX Back Row",                        PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("TRX"),             DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="بارفیکس با وزنه",                 EnglishName="Weighted Pull-Up",                    PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="لت پول‌داون معکوس",               EnglishName="Reverse Grip Lat Pulldown",           PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="زیربغل دستگاه",                   EnglishName="Machine Row",                         PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="هایپراکستنشن با وزنه",            EnglishName="Weighted Hyperextension",             PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ددلیفت با دمبل",                  EnglishName="Dumbbell Deadlift",                   PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="زیربغل با کتل‌بل",               EnglishName="Kettlebell Row",                      PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== سرشانه - تکمیلی ====================
new() { Name="ال‌اکسیدنتال پرس",               EnglishName="Landmine Press",                      PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرس سرشانه با باند",              EnglishName="Resistance Band Shoulder Press",      PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="نشر جانب با باند",                EnglishName="Resistance Band Lateral Raise",       PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="نشر جلو با کابل",                 EnglishName="Cable Front Raise",                   PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پرس سرشانه با کتل‌بل",           EnglishName="Kettlebell Shoulder Press",           PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرس Z با هالتر",                  EnglishName="Z-Press",                             PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="اپرایت رو با کابل",               EnglishName="Cable Upright Row",                   PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پرس سرشانه تک دست",              EnglishName="Single-Arm Shoulder Press",           PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="نشر خم با کابل",                  EnglishName="Cable Rear Delt Fly",                 PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پشتی دلتوئید دستگاه",            EnglishName="Rear Delt Machine",                   PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="چرخش خارجی شانه با کابل",        EnglishName="Cable External Rotation",             PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="چرخش داخلی شانه با کابل",        EnglishName="Cable Internal Rotation",             PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== جلو بازو - تکمیلی ====================
new() { Name="جلو بازو با باند مقاومتی",        EnglishName="Resistance Band Curl",                PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو برگمن",                  EnglishName="Zottman Curl",                        PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="جلو بازو روی میز شیب‌دار",        EnglishName="Spider Curl",                         PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="جلو بازو کابل بالا",              EnglishName="High Cable Curl",                     PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو لاری با کابل",           EnglishName="Cable Preacher Curl",                 PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو دو دست با کابل",         EnglishName="Double Cable Curl",                   PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="جلو بازو با دمبل چرخشی",          EnglishName="Supinating Dumbbell Curl",            PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو کتل‌بل",                 EnglishName="Kettlebell Curl",                     PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== پشت بازو - تکمیلی ====================
new() { Name="پشت بازو JM پرس",                 EnglishName="JM Press",                            PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پشت بازو با باند مقاومتی",        EnglishName="Resistance Band Tricep Extension",    PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پشت بازو EZ اسکال کراشر",        EnglishName="EZ Bar Skull Crusher",                PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اکستنشن پشت بازو با دمبل خوابیده",EnglishName="Lying Dumbbell Tricep Extension",    PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پشت بازو هالتر بالای سر",         EnglishName="Barbell Overhead Tricep Extension",   PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="دیپ روی میز",                     EnglishName="Bench Dip",                           PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پشت بازو دستگاه",                 EnglishName="Tricep Machine Extension",            PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== چهارسر - تکمیلی ====================
new() { Name="اسکوات هاک با هالتر",             EnglishName="Barbell Hack Squat",                  PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اسکوات اسمیت مشین",               EnglishName="Smith Machine Squat",                 PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="لانج با هالتر",                   EnglishName="Barbell Lunge",                       PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اسکوات پرشی",                     EnglishName="Jump Squat",                          PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="لانج با باند مقاومتی",            EnglishName="Resistance Band Lunge",               PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اسکوات با توپ سوئیسی",            EnglishName="Swiss Ball Squat",                    PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("توپ سوئیسی"),      DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کاسین پا",                        EnglishName="Terminal Knee Extension",             PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="استپ آپ با هالتر",                EnglishName="Barbell Step-Up",                     PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اکستنشن تک پا",                   EnglishName="Single-Leg Extension",                PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="لانج اسپلیت با جامپ",             EnglishName="Split Jump Lunge",                    PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== همسترینگ - تکمیلی ====================
new() { Name="کرل پا با توپ سوئیسی",            EnglishName="Swiss Ball Leg Curl",                 PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("توپ سوئیسی"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="کرل پا با باند مقاومتی",          EnglishName="Resistance Band Leg Curl",            PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ددلیفت رومانیایی تک پا با کابل",  EnglishName="Single-Leg Cable Romanian Deadlift",  PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ددلیفت با هگ بار",                EnglishName="Trap Bar Deadlift",                   PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرس پا با پاشنه",                 EnglishName="High Foot Placement Leg Press",       PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اسلاید همسترینگ",                 EnglishName="Hamstring Slide Curl",                PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== سرینی - تکمیلی ====================
new() { Name="هیپ تراست تک پا",                 EnglishName="Single-Leg Hip Thrust",               PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="هیپ تراست با کابل",               EnglishName="Cable Hip Thrust",                    PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="کیک‌بک با باند",                  EnglishName="Banded Kickback",                     PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="لانج بلغاری با هالتر",            EnglishName="Barbell Bulgarian Split Squat",       PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="مینی‌باند راه رفتن",              EnglishName="Mini Band Walk",                      PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پروگ پامپ",                       EnglishName="Frog Pump",                           PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ددلیفت اسمیت مشین",               EnglishName="Smith Machine Deadlift",              PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== ساق - تکمیلی ====================
new() { Name="ساق با دستگاه پرس پا",            EnglishName="Leg Press Calf Raise",                PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ساق ایستاده با هالتر",            EnglishName="Barbell Standing Calf Raise",         PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ساق با باند مقاومتی",             EnglishName="Resistance Band Calf Raise",          PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ساق پرشی",                        EnglishName="Jump Calf Raise",                     PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="دورسی‌فلکشن با باند",             EnglishName="Banded Dorsiflexion",                 PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ساق نشسته با هالتر",              EnglishName="Barbell Seated Calf Raise",           PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== هسته - تکمیلی ====================
new() { Name="اکستنشن پشت روی توپ",             EnglishName="Swiss Ball Back Extension",           PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("توپ سوئیسی"),      DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کرانچ روی توپ سوئیسی",            EnglishName="Swiss Ball Crunch",                   PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("توپ سوئیسی"),      DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پلانک با توپ سوئیسی",             EnglishName="Swiss Ball Plank",                    PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("توپ سوئیسی"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پلانک با کابل",                   EnglishName="Cable Plank",                         PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پلانک با بالا بردن پا",           EnglishName="Plank Leg Raise",                     PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پلانک با بالا بردن دست",          EnglishName="Plank Arm Raise",                     PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پلانک معکوس",                     EnglishName="Reverse Plank",                       PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="توبی کرانچ",                      EnglishName="Tuck Crunch",                         PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="چرخش روسی با توپ طبی",            EnglishName="Medicine Ball Russian Twist",         PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("توپ طبی"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرتاب توپ طبی به دیوار",          EnglishName="Medicine Ball Slam",                  PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("توپ طبی"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="چرخش با کابل ایستاده",            EnglishName="Standing Cable Rotation",             PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="چرخش با کابل نشسته",              EnglishName="Seated Cable Rotation",               PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پلانک شنا",                       EnglishName="Plank to Push-Up",                    PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="سیشور کیک",                       EnglishName="Scissor Kick",                        PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="فلاتر کیک",                       EnglishName="Flutter Kick",                        PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="توک جامپ",                        EnglishName="Tuck Jump",                           PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== تمام بدن - تکمیلی ====================
new() { Name="ددلیفت کلاسیک",                   EnglishName="Conventional Deadlift",               PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ترکیبی پشو-بارفیکس",              EnglishName="Push-Pull Combo",                     PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="فارمر کری",                       EnglishName="Farmer's Carry",                      PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="فارمر کری با کتل‌بل",             EnglishName="Kettlebell Farmer's Carry",           PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ترایسپ پرس پا",                   EnglishName="Thruster",                            PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="مان میکرز",                       EnglishName="Man Makers",                          PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="ترکیب ددلیفت-پرس",               EnglishName="Deadlift to Press",                   PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اسکوات-پرس",                      EnglishName="Squat to Press",                      PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="رپه‌ایر",                         EnglishName="Renegade Row",                        PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="کتل‌بل ترکیبی",                   EnglishName="Kettlebell Complex",                  PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="ارتفاع پرش",                      EnglishName="Depth Jump",                          PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="اسلد پوش",                        EnglishName="Sled Push",                           PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اسلد پول",                        EnglishName="Sled Pull",                           PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="طناب جنگی",                       EnglishName="Battle Rope",                         PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="طناب جنگی موجی",                  EnglishName="Battle Rope Waves",                   PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="سانداگ کری",                      EnglishName="Sandbag Carry",                       PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="تایر فلیپ",                       EnglishName="Tire Flip",                           PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("پیشرفته"), IsActive=true },

// ==================== کاردیو - تکمیلی ====================
new() { Name="پرش جانبی بلند",                  EnglishName="Lateral Bound",                       PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اسپیرینت با مقاومت",              EnglishName="Resisted Sprint",                     PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="دو رفت و برگشت",                  EnglishName="Shuttle Run",                         PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="بالا پایین پله",                  EnglishName="Step Up and Down",                    PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ایروبیک آبی",                     EnglishName="Water Aerobics",                      PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="رکاب زدن شدید",                   EnglishName="High Intensity Cycling",              PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ورزش با طناب لاسو",               EnglishName="Jump Rope Double Under",              PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("طناب"),            DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="پرش صندوق پایین",                 EnglishName="Box Drop Jump",                       PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="دو نفری با باند",                 EnglishName="Partner Resisted Run",                PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== کمر پایینی - تکمیلی ====================
new() { Name="کوبرا کشش",                       EnglishName="Cobra Stretch",                       PrimaryMuscleGroupId=MG("کمر پایینی"),EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پیچش ستون فقرات",                 EnglishName="Spinal Twist",                        PrimaryMuscleGroupId=MG("کمر پایینی"),EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="مک‌کنزی اکستنشن",                EnglishName="McKenzie Extension",                  PrimaryMuscleGroupId=MG("کمر پایینی"),EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="هولوهاگ پوزیشن",                 EnglishName="Child's Pose",                        PrimaryMuscleGroupId=MG("کمر پایینی"),EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ددلیفت رومانیایی با دمبل",        EnglishName="Dumbbell Romanian Deadlift",          PrimaryMuscleGroupId=MG("کمر پایینی"),EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اکستنشن کمر با توپ",              EnglishName="Swiss Ball Back Extension",           PrimaryMuscleGroupId=MG("کمر پایینی"),EquipmentTypeId=EQ("توپ سوئیسی"),      DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== تله‌پز - تکمیلی ====================
new() { Name="شراگ با هگ بار",                  EnglishName="Trap Bar Shrug",                      PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="شراگ جلو با دمبل",                EnglishName="Front Dumbbell Shrug",                PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="شراگ نشسته با دمبل",              EnglishName="Seated Dumbbell Shrug",               PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پاور اسنچ",                       EnglishName="Power Snatch",                        PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="شراگ با باند",                    EnglishName="Resistance Band Shrug",               PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== ساعد - تکمیلی ====================
new() { Name="آویزان شدن از میله",              EnglishName="Dead Hang",                           PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="فارمر کری تک دست",               EnglishName="Single-Arm Farmer's Carry",           PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="چنگ زدن به حوله",                 EnglishName="Towel Pull-Up",                       PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="کرل مچ با کابل",                  EnglishName="Cable Wrist Curl",                    PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== تمرینات تعادلی (Balance) ====================
new() { Name="ایستادن روی یک پا",               EnglishName="Single-Leg Stand",                    PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پرس دمبل روی توپ",                EnglishName="Stability Ball Dumbbell Press",       PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("توپ سوئیسی"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اسکوات روی بوسو",                 EnglishName="BOSU Ball Squat",                     PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پشو روی بوسو",                    EnglishName="BOSU Ball Push-Up",                   PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="لانج روی بوسو",                   EnglishName="BOSU Ball Lunge",                     PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرس سرشانه روی توپ سوئیسی",      EnglishName="Swiss Ball Shoulder Press",           PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("توپ سوئیسی"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="جلو بازو روی توپ سوئیسی",        EnglishName="Swiss Ball Bicep Curl",               PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("توپ سوئیسی"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== کشش و انعطاف (Stretching) ====================
new() { Name="کشش چهارسر",                      EnglishName="Quad Stretch",                        PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کشش همسترینگ",                    EnglishName="Hamstring Stretch",                   PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کشش ساق",                         EnglishName="Calf Stretch",                        PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کشش سینه",                        EnglishName="Chest Stretch",                       PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کشش سرشانه جلو",                  EnglishName="Anterior Shoulder Stretch",           PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کشش لت",                          EnglishName="Lat Stretch",                         PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کشش هیپ فلکسور",                  EnglishName="Hip Flexor Stretch",                  PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کشش پیریفورمیس",                  EnglishName="Piriformis Stretch",                  PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کشش جلو بازو",                    EnglishName="Bicep Stretch",                       PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کشش پشت بازو",                    EnglishName="Tricep Stretch",                      PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کشش گردن",                        EnglishName="Neck Stretch",                        PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کشش مچ",                          EnglishName="Wrist Stretch",                       PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== گرم‌کردن (Warm-Up) ====================
new() { Name="لگد چرخشی",                       EnglishName="Leg Swing",                           PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="چرخش شانه",                       EnglishName="Arm Circle",                          PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="چرخش مچ پا",                      EnglishName="Ankle Circle",                        PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="چرخش لگن",                        EnglishName="Hip Circle",                          PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="بالا آوردن زانو",                 EnglishName="High Knee",                           PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="لگد پشتی",                        EnglishName="Butt Kick",                           PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="حرکت اینچ‌ورم",                   EnglishName="Inchworm",                            PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="حرکت جهشی جانبی",                 EnglishName="Side Shuffle",                        PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پرش اسکیتر",                      EnglishName="Skater Jump",                         PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ووڈچاپر با دمبل",                 EnglishName="Dumbbell Woodchop",                   PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ووڈچاپر با کابل",                 EnglishName="Cable Woodchop",                      PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },



// ==================== سینه - بلوک ۴ ====================
new() { Name="پشو با دست باز",                  EnglishName="Wide Push-Up",                        PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پشو با دست جمع",                  EnglishName="Narrow Push-Up",                      PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پرس سینه با کتل‌بل",             EnglishName="Kettlebell Chest Press",              PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پشو با پاها روی دیوار",           EnglishName="Wall Push-Up",                        PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پشو کج",                          EnglishName="Decline Push-Up",                     PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="فلای با دمبل شیب بالا",           EnglishName="Incline Dumbbell Fly",                PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="فلای با دمبل شیب پایین",          EnglishName="Decline Dumbbell Fly",                PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرس سینه اسمیت مشین شیب",        EnglishName="Smith Machine Incline Press",         PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پرس سینه هماهنگ",                 EnglishName="Svend Press",                         PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پشو اسپایدرمن",                   EnglishName="Spiderman Push-Up",                   PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== پشت - بلوک ۴ ====================
new() { Name="زیربغل T-Bar",                    EnglishName="T-Bar Row",                           PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پول‌آپ با گریپ پهن",              EnglishName="Wide Grip Pull-Up",                   PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پول‌آپ با گریپ جمع",              EnglishName="Close Grip Pull-Up",                  PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="چین‌آپ با نوتر گریپ",             EnglishName="Neutral Grip Chin-Up",                PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="زیربغل با دمبل یک طرف",           EnglishName="Unilateral Dumbbell Row",             PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کابل رو ایستاده",                 EnglishName="Standing Cable Row",                  PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="فیس‌پول با باند",                 EnglishName="Resistance Band Face Pull",           PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="لت پول‌داون با باند",             EnglishName="Resistance Band Lat Pulldown",        PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="زیربغل سوپینه با دمبل",           EnglishName="Supine Dumbbell Row",                 PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="کشش لت با کابل یک طرف",          EnglishName="Single-Arm Lat Pulldown",             PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== سرشانه - بلوک ۴ ====================
new() { Name="نشر جانب دستگاه",                 EnglishName="Machine Lateral Raise",               PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پرس سرشانه اسمیت مشین",           EnglishName="Smith Machine Shoulder Press",        PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="نشر جانب نشسته با کابل",          EnglishName="Seated Cable Lateral Raise",          PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پوش‌پرس با هالتر",                EnglishName="Push Press",                          PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="نشر جانب خم با دمبل",             EnglishName="Bent-Over Lateral Raise",             PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پرس آرنولد با کتل‌بل",            EnglishName="Kettlebell Arnold Press",             PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="لندماین رو",                      EnglishName="Landmine Row",                        PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="نشر خم با باند مقاومتی",          EnglishName="Resistance Band Rear Delt Raise",     PrimaryMuscleGroupId=MG("سرشانه"),     EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== جلو بازو - بلوک ۴ ====================
new() { Name="جلو بازو لاری با هالتر",          EnglishName="Barbell Preacher Curl",               PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="جلو بازو متناوب با دمبل",         EnglishName="Alternating Dumbbell Curl",           PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو چکشی با کابل",           EnglishName="Cable Hammer Curl",                   PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو دستگاه",                 EnglishName="Machine Bicep Curl",                  PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو تمرکزی نشسته",           EnglishName="Seated Concentration Curl",           PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جلو بازو اسکات با هالتر",         EnglishName="Scott Curl",                          PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="جلو بازو ۲۱",                     EnglishName="21s Curl",                            PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="جلو بازو کابل تک دست",            EnglishName="Single-Arm Cable Curl",               PrimaryMuscleGroupId=MG("جلو بازو"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== پشت بازو - بلوک ۴ ====================
new() { Name="پرس سینه محدوده کم",              EnglishName="Close Grip Bench Press",              PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اکستنشن پشت بازو تک دست کابل",   EnglishName="Single-Arm Cable Tricep Extension",   PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پشت بازو لاری با دمبل",           EnglishName="Dumbbell Kickback Prone",             PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پوش‌داون با طناب",                EnglishName="Rope Pushdown",                       PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="دیپ پارالل",                      EnglishName="Parallel Bar Dip",                    PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پشت بازو با کتل‌بل",             EnglishName="Kettlebell Tricep Extension",          PrimaryMuscleGroupId=MG("پشت بازو"),   EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== چهارسر ران - بلوک ۴ ====================
new() { Name="اسکوات گاوی",                     EnglishName="Goblet Squat",                        PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اسکوات کوبایی",                   EnglishName="Cuban Squat",                         PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اسکوات با دمبل",                  EnglishName="Dumbbell Squat",                      PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اسکوات با کابل",                  EnglishName="Cable Squat",                         PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پیستول اسکوات",                   EnglishName="Pistol Squat",                        PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="لانج معکوس با دمبل",              EnglishName="Reverse Dumbbell Lunge",              PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="لانج جانب با دمبل",               EnglishName="Lateral Dumbbell Lunge",              PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="وال سیت با دمبل",                 EnglishName="Wall Sit with Dumbbell",              PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="لگ پرس تک پا",                    EnglishName="Single-Leg Press",                    PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اسکوات زیرو",                     EnglishName="Zercher Squat",                       PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },

// ==================== همسترینگ - بلوک ۴ ====================
new() { Name="نوردیک کرل",                      EnglishName="Nordic Curl",                         PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="گود‌مورنینگ با دمبل",             EnglishName="Dumbbell Good Morning",               PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ددلیفت سوماتی",                   EnglishName="Sumo Deadlift",                       PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="کرل پا با کابل",                  EnglishName="Cable Leg Curl",                      PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ددلیفت با کتل‌بل",               EnglishName="Kettlebell Deadlift",                 PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="گود‌مورنینگ نشسته",               EnglishName="Seated Good Morning",                 PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ددلیفت قفل جهشی",                 EnglishName="Snatch Grip Deadlift",                PrimaryMuscleGroupId=MG("همسترینگ"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },

// ==================== سرینی - بلوک ۴ ====================
new() { Name="هیپ اکستنشن با کابل",             EnglishName="Cable Hip Extension",                 PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اسکوات سومو با دمبل",             EnglishName="Dumbbell Sumo Squat",                 PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="هیپ تراست با دمبل",               EnglishName="Dumbbell Hip Thrust",                 PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پل سرینی با یک پا",               EnglishName="Single-Leg Glute Bridge",             PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کیک‌بک با کابل",                  EnglishName="Cable Kickback",                      PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کلم‌شل",                          EnglishName="Clamshell",                           PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کلم‌شل با باند",                  EnglishName="Banded Clamshell",                    PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اسکوات سوماتی با هالتر",          EnglishName="Barbell Sumo Squat",                  PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== ساق پا - بلوک ۴ ====================
new() { Name="ساق یک پا با دمبل",               EnglishName="Single-Leg Dumbbell Calf Raise",      PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ساق با کتل‌بل",                   EnglishName="Kettlebell Calf Raise",               PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ساق روی پله تک پا",               EnglishName="Single-Leg Step Calf Raise",          PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ساق خرسی",                        EnglishName="Donkey Calf Raise",                   PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ساق با اسمیت مشین",               EnglishName="Smith Machine Calf Raise",            PrimaryMuscleGroupId=MG("ساق"),        EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== هسته - بلوک ۴ ====================
new() { Name="توبی کرانچ معکوس",                EnglishName="Reverse Tuck Crunch",                 PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="V-آپ",                             EnglishName="V-Up",                                PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ستاره کرانچ",                     EnglishName="Star Crunch",                         PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اسپایدرمن پلانک",                 EnglishName="Spiderman Plank",                     PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پل با چرخش",                      EnglishName="Bridge with Rotation",                PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="کابل کرانچ زانو زده",             EnglishName="Kneeling Cable Crunch",               PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اب‌ویل",                          EnglishName="Ab Wheel Rollout",                    PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="اب‌ویل ایستاده",                  EnglishName="Standing Ab Wheel Rollout",           PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="پلانک با دست زدن",                EnglishName="Plank Shoulder Tap",                  PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="کرانچ کابل ایستاده",              EnglishName="Standing Cable Crunch",               PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="توپ طبی کرانچ",                   EnglishName="Medicine Ball Crunch",                PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("توپ طبی"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کرانچ بایسیکل معکوس",             EnglishName="Reverse Bicycle Crunch",              PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== تله‌پز - بلوک ۴ ====================
new() { Name="شراگ پشت با هالتر",               EnglishName="Behind-the-Back Shrug",               PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="شراگ با کابل",                    EnglishName="Cable Shrug",                         PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="شراگ با کتل‌بل",                 EnglishName="Kettlebell Shrug",                    PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اسمیت مشین شراگ",                EnglishName="Smith Machine Shrug",                 PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== ساعد - بلوک ۴ ====================
new() { Name="پشت مچ با هالتر",                 EnglishName="Barbell Reverse Wrist Curl",          PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کرل مچ با باند",                  EnglishName="Resistance Band Wrist Curl",          PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="فشار توپ اسفنجی",                 EnglishName="Stress Ball Squeeze",                 PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="نگه‌داشتن وزنه با انگشتان",       EnglishName="Pinch Grip Hold",                     PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("دمبل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="آویزان با حوله",                  EnglishName="Towel Dead Hang",                     PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== ادداکتور - بلوک ۴ ====================
new() { Name="جمع کردن پاها با دستگاه",         EnglishName="Machine Hip Adduction",               PrimaryMuscleGroupId=MG("ادداکتور"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جمع کردن پاها با باند",           EnglishName="Banded Hip Adduction",                PrimaryMuscleGroupId=MG("ادداکتور"),   EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="اسکوات سومو با زانوبند",          EnglishName="Banded Sumo Squat",                   PrimaryMuscleGroupId=MG("ادداکتور"),   EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="لانج جانب با باند",               EnglishName="Banded Lateral Lunge",                PrimaryMuscleGroupId=MG("ادداکتور"),   EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="جمع کردن پاها با کابل",           EnglishName="Cable Hip Adduction",                 PrimaryMuscleGroupId=MG("ادداکتور"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== ابداکتور - بلوک ۴ ====================
new() { Name="باز کردن پاها با دستگاه",         EnglishName="Machine Hip Abduction",               PrimaryMuscleGroupId=MG("ابداکتور"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="باز کردن پاها با باند",           EnglishName="Banded Hip Abduction",                PrimaryMuscleGroupId=MG("ابداکتور"),   EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="باز کردن پاها با کابل",           EnglishName="Cable Hip Abduction",                 PrimaryMuscleGroupId=MG("ابداکتور"),   EquipmentTypeId=EQ("کابل"),            DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="راه رفتن جانب با باند",           EnglishName="Lateral Band Walk",                   PrimaryMuscleGroupId=MG("ابداکتور"),   EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="باز کردن پا در حالت خوابیده",     EnglishName="Side-Lying Hip Abduction",            PrimaryMuscleGroupId=MG("ابداکتور"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== تمام بدن - بلوک ۴ ====================
new() { Name="پاور کلین",                       EnglishName="Power Clean",                         PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="پاور جرک",                        EnglishName="Power Jerk",                          PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="کلین اند جرک",                    EnglishName="Clean and Jerk",                      PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="اسنچ با هالتر",                   EnglishName="Barbell Snatch",                      PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="کتل‌بل کلین",                     EnglishName="Kettlebell Clean",                    PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="کتل‌بل جرک",                      EnglishName="Kettlebell Jerk",                     PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="کتل‌بل اسنچ",                     EnglishName="Kettlebell Snatch",                   PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("کتل‌بل"),          DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="سانداگ اسنچ",                     EnglishName="Sandbag Snatch",                      PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="اسکوات اورهد",                    EnglishName="Overhead Squat",                      PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="برپی با بارفیکس",                 EnglishName="Burpee Pull-Up",                      PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="برپی با جامپ‌اوور",               EnglishName="Burpee Box Jump Over",                PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="ترکیبی با باند",                  EnglishName="Band Complex",                        PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("باند مقاومتی"),    DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="هالتر کمپلکس",                    EnglishName="Barbell Complex",                     PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("هالتر"),           DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="سرویت ترکیبی",                    EnglishName="Circuit Training",                    PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="سانداگ کلین",                     EnglishName="Sandbag Clean",                       PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرس اورهد با طناب",               EnglishName="Battle Rope Overhead Press",          PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },

// ==================== کاردیو - بلوک ۴ ====================
new() { Name="دوی سرعت ۱۰۰ متر",               EnglishName="100m Sprint",                         PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="تمرین HIIT",                      EnglishName="HIIT Training",                       PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="تمرین تابتا",                     EnglishName="Tabata",                              PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("پیشرفته"), IsActive=true },
new() { Name="دوچرخه ثابت استقامتی",            EnglishName="Steady State Cycling",                PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("دستگاه"),          DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پیاده‌روی سریع",                  EnglishName="Brisk Walking",                       PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="دو با مانع",                      EnglishName="Hurdle Run",                          PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="پرش با طناب تک پا",               EnglishName="Single-Leg Jump Rope",                PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("طناب"),            DifficultyLevelId=DL("متوسط"),   IsActive=true },
new() { Name="ایروبیک پله",                     EnglishName="Step Aerobics",                       PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ورزش در آب",                      EnglishName="Aqua Jogging",                        PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="ترامپولین",                       EnglishName="Trampoline Jumping",                  PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("ابزار دیگر"),      DifficultyLevelId=DL("مبتدی"),   IsActive=true },

// ==================== گرم‌کردن و سرد کردن - بلوک ۴ ====================
new() { Name="چرخش گردن",                       EnglishName="Neck Roll",                           PrimaryMuscleGroupId=MG("تله‌پز"),     EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="چرخش مچ دست",                     EnglishName="Wrist Circle",                        PrimaryMuscleGroupId=MG("ساعد"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="دپ‌سکوات گرم‌کردن",              EnglishName="Deep Squat Hold",                     PrimaryMuscleGroupId=MG("چهارسر ران"), EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="باز و بسته کردن قفسه سینه",      EnglishName="Chest Opener",                        PrimaryMuscleGroupId=MG("سینه"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پیچش ستون ایستاده",               EnglishName="Standing Torso Twist",                PrimaryMuscleGroupId=MG("هسته"),       EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="حرکت کات",                        EnglishName="Cat-Cow Stretch",                     PrimaryMuscleGroupId=MG("کمر پایینی"),EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="سرپایی به زمین",                  EnglishName="Sun Salutation",                      PrimaryMuscleGroupId=MG("تمام بدن"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کشش پروانه‌ای",                   EnglishName="Butterfly Stretch",                   PrimaryMuscleGroupId=MG("ادداکتور"),   EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="کشش T نشسته",                     EnglishName="Seated T-Stretch",                    PrimaryMuscleGroupId=MG("پشت"),        EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="پیروول",                          EnglishName="Pigeon Pose",                         PrimaryMuscleGroupId=MG("سرینی"),      EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },
new() { Name="درازکش بازکننده",                 EnglishName="Supine Twist",                        PrimaryMuscleGroupId=MG("کمر پایینی"),EquipmentTypeId=EQ("وزن بدن"),         DifficultyLevelId=DL("مبتدی"),   IsActive=true },

        };

        await context.SeedIfNotExists(
            exercises,
            e => x => x.Name == e.Name && x.GymId == null);





    }





}
