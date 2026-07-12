using FitCore.Domain.Entities.Announcements;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Help;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.NutritionProgram.Food;
using FitCore.Domain.Entities.NutritionProgram.NutritionMeal;
using FitCore.Domain.Entities.NutritionProgram.NutritionMealItem;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgramDay;
using FitCore.Domain.Entities.ProgramRequest;
using FitCore.Domain.Entities.Provinces;
using FitCore.Domain.Entities.Setings;
using FitCore.Domain.Entities.Tickets;
using FitCore.Domain.Entities.TrainingProgram;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using System.Threading;
using System.Threading.Tasks;

namespace FitCore.Application.Contexts
{
    public interface IDataBaseContext
    {
        DbSet<Gym> Gyms { get; set; }

        DbSet<Member> Members { get; set; }
        DbSet<Setings> Setings { get; set; }
        public DbSet<UserOtpCode> UserOtpCodes { get; set; }

        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }

        DatabaseFacade Database { get; }

        DbSet<AppUser> Users { get; set; }
        DbSet<MemberBodyMeasurement> memberBodyMeasurements { get; set; }
        DbSet<ExperienceLevel> experiences { get; set; }
        DbSet<ActivityLevel> activityLevels { get; set; }

        DbSet<IdentityRole<long>> Roles { get; set; }

        DbSet<IdentityUserRole<long>> UserRoles { get; set; }


        DbSet<Announcement> Announcements { get; set; }

        DbSet<AnnouncementRole> AnnouncementRoles { get; set; }

        DbSet<AnnouncementGym> AnnouncementGyms { get; set; }

        DbSet<AnnouncementView> AnnouncementViews { get; set; }


        //===============NutritionProgram
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodUnitConversion> FoodUnitConversions { get; set; }

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


        //-------------  TrainingPrograms Start-------------
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
        //-------------  TrainingPrograms End-------------
        public DbSet<HelpContent> HelpContents { get; set; }
        public DbSet<ProgramRequest> ProgramRequests { get; set; }



        //-------------  Ticket Start -------------
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketMessage> TicketMessages { get; set; }

        //-------------  Ticket End -------------







        int SaveChanges();

        Task<int> SaveChangesAsync(
                CancellationToken cancellationToken = default);
    }
}
