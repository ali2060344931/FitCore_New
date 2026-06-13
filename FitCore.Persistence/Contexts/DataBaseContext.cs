using FitCore.Application.Contexts;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.NutritionProgram.Food;
using FitCore.Domain.Entities.NutritionProgram.NutritionMeal;
using FitCore.Domain.Entities.NutritionProgram.NutritionMealItem;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgramDay;
using FitCore.Domain.Entities.Provinces;
using FitCore.Domain.Entities.Setings;
using FitCore.Domain.Entities.TrainingProgram;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System.Threading;
using System.Threading.Tasks;

namespace FitCore.Persistence.Contexts
{
    public class DataBaseContext :
        IdentityDbContext<AppUser, IdentityRole<long>, long>,
        IDataBaseContext
    {
        public DataBaseContext(
            DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        public DbSet<Gym> Gyms { get; set; }

        public DbSet<Member> Members { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Setings> Setings { get; set; }

        public DbSet<UserOtpCode> UserOtpCodes { get; set; }

        public DbSet<Province> Provinces { get; set; }

        public DbSet<City> Cities { get; set; }


        //===============NutritionProgram
        public DbSet<Food> Foods { get; set; }
        public DbSet<NutritionUnitType> NutritionUnitTypes { get; set; }
        public DbSet<FoodCategoryType> FoodCategoryTypes { get; set; }
        public DbSet<NutritionMeal> NutritionMeals { get; set; }
        public DbSet<MealType> MealTypes { get; set; }
        public DbSet<NutritionMealItem> NutritionMealItems { get; set; }
        public DbSet<NutritionProgram> NutritionPrograms { get; set; }
        public DbSet<NutritionProgramType> NutritionProgramTypes { get; set; }
        public DbSet<GoalType> GetGoalTypes { get; set; }
        public DbSet<NutritionProgramDay> NutritionProgramDays { get; set; }
        //===============
        public DbSet<MemberBodyMeasurement> memberBodyMeasurements { get; set; }
        public DbSet<ExperienceLevel> experiences { get; set; }
        public DbSet<ActivityLevel> activityLevels { get; set; }

        //===============TrainingProgram
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<TrainingProgramType> TrainingProgramTypes { get; set; }
        public DbSet<TrainingGoalType> TrainingGoalTypes { get; set; }
        public DbSet<TrainingDay> TrainingDays { get; set; }
        public DbSet<TrainingDayType> TrainingDayTypes { get; set; }
        public DbSet<TrainingExerciseItem> TrainingExerciseItems { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<MuscleGroup> MuscleGroups { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<ExerciseDifficultyLevel> ExerciseDifficultyLevels { get; set; }
        //===============

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ApplyQueryFilter(modelBuilder);
        }

        private void ApplyQueryFilter(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gym>()
                .HasQueryFilter(x => !x.IsRemoved);

            modelBuilder.Entity<Member>()
                .HasQueryFilter(x => !x.IsRemoved);

            modelBuilder.Entity<Member>()
                .HasOne(m => m.AppUser)
                .WithOne(u => u.Member)
                .HasForeignKey<Member>(m => m.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);


            //NutritionProgram


            //*************
            // ---------- NutritionProgram ----------
            modelBuilder.Entity<FitCore.Domain.Entities.NutritionProgram.NutritionProgram.NutritionProgram>(entity =>
            {
                entity.HasOne(x => x.Member)
                      .WithMany() // اگر در Member کالکشن NutritionPrograms نداری
                      .HasForeignKey(x => x.MemberId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.Gym)
                      .WithMany()
                      .HasForeignKey(x => x.GymId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.CreatedByUser)
                      .WithMany()
                      .HasForeignKey(x => x.CreatedByUserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.ProgramType)
                      .WithMany(x => x.nutritionPrograms)
                      .HasForeignKey(x => x.ProgramTypeId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.GoalType)
                      .WithMany(x => x.nutritionPrograms)
                      .HasForeignKey(x => x.GoalTypeId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // ---------- NutritionProgramDay ----------
            modelBuilder.Entity<FitCore.Domain.Entities.NutritionProgram.NutritionProgramDay.NutritionProgramDay>(entity =>
            {
                entity.HasOne(x => x.NutritionProgram)
                      .WithMany(x => x.Days)
                      .HasForeignKey(x => x.NutritionProgramId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // ---------- NutritionMeal ----------
            modelBuilder.Entity<FitCore.Domain.Entities.NutritionProgram.NutritionMeal.NutritionMeal>(entity =>
            {
                entity.HasOne(x => x.NutritionProgramDay)
                      .WithMany(x => x.Meals)
                      .HasForeignKey(x => x.NutritionProgramDayId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.MealType)
                      .WithMany(x => x.nutritionMeals)
                      .HasForeignKey(x => x.MealTypeId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
            // ---------- NutritionMealItem ----------
            modelBuilder.Entity<FitCore.Domain.Entities.NutritionProgram.NutritionMealItem.NutritionMealItem>(entity =>
            {
                entity.HasOne(x => x.NutritionMeal)
                      .WithMany(x => x.Items)
                      .HasForeignKey(x => x.NutritionMealId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.Food)
                      .WithMany() // چون در Food کالکشن MealItems نداری
                      .HasForeignKey(x => x.FoodId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.UnitType)
      .WithMany()
      .HasForeignKey(x => x.UnitTypeId)
      .OnDelete(DeleteBehavior.NoAction);
                // نکته: شما UnitType در NutritionMealItem تعریف کرده‌ای ولی UnitTypeId نداری.
                // پس فعلاً رابطه‌ای برای UnitType ساخته نمی‌شود. اگر می‌خواهی FK باشد باید UnitTypeId اضافه کنی.
            });

            // ---------- Food ----------
            modelBuilder.Entity<FitCore.Domain.Entities.NutritionProgram.Food.Food>(entity =>
            {
                entity.HasOne(x => x.DefaultUnit)
                      .WithMany(x => x.food)
                      .HasForeignKey(x => x.DefaultUnitId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.CategoryType)
                      .WithMany(x => x.food)
                      .HasForeignKey(x => x.CategoryTypeId)
                      .OnDelete(DeleteBehavior.NoAction);


            });
            //*************

            // ---------- TrainingProgram ----------
            modelBuilder.Entity<FitCore.Domain.Entities.TrainingProgram.TrainingProgram>(entity =>
            {
                entity.HasQueryFilter(x => !x.IsRemoved);

                entity.HasOne(x => x.Member)
                      .WithMany()
                      .HasForeignKey(x => x.MemberId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.Gym)
                      .WithMany()
                      .HasForeignKey(x => x.GymId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.CreatedByUser)
                      .WithMany()
                      .HasForeignKey(x => x.CreatedByUserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.TrainingProgramType)
                      .WithMany(x => x.TrainingPrograms)
                      .HasForeignKey(x => x.TrainingProgramTypeId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.TrainingGoalType)
                      .WithMany(x => x.TrainingPrograms)
                      .HasForeignKey(x => x.TrainingGoalTypeId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // ---------- TrainingDay ----------
            modelBuilder.Entity<TrainingDay>(entity =>
            {
                entity.HasQueryFilter(x => !x.IsRemoved);

                entity.HasOne(x => x.TrainingProgram)
                      .WithMany(x => x.Days)
                      .HasForeignKey(x => x.TrainingProgramId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.DayType)
                      .WithMany(x => x.TrainingDays)
                      .HasForeignKey(x => x.DayTypeId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // ---------- TrainingExerciseItem ----------
            modelBuilder.Entity<TrainingExerciseItem>(entity =>
            {
                entity.HasQueryFilter(x => !x.IsRemoved);

                entity.HasOne(x => x.TrainingDay)
                      .WithMany(x => x.ExerciseItems)
                      .HasForeignKey(x => x.TrainingDayId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.Exercise)
                      .WithMany(x => x.ExerciseItems)
                      .HasForeignKey(x => x.ExerciseId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // ---------- Exercise ----------
            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasQueryFilter(x => !x.IsRemoved);

                entity.HasOne(x => x.Gym)
                      .WithMany()
                      .HasForeignKey(x => x.GymId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.PrimaryMuscleGroup)
                      .WithMany(x => x.Exercises)
                      .HasForeignKey(x => x.PrimaryMuscleGroupId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.EquipmentType)
                      .WithMany(x => x.Exercises)
                      .HasForeignKey(x => x.EquipmentTypeId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.DifficultyLevel)
                      .WithMany(x => x.Exercises)
                      .HasForeignKey(x => x.DifficultyLevelId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
            //*************TrainingProgram

        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}