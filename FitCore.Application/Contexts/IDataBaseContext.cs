using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.NutritionProgram.Food;
using FitCore.Domain.Entities.NutritionProgram.NutritionMeal;
using FitCore.Domain.Entities.NutritionProgram.NutritionMealItem;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgramDay;
using FitCore.Domain.Entities.Provinces;
using FitCore.Domain.Entities.Setings;
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

        DbSet<IdentityRole<long>> Roles { get; set; }

        DbSet<IdentityUserRole<long>> UserRoles { get; set; }


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

        int SaveChanges();

        Task<int> SaveChangesAsync(
                CancellationToken cancellationToken = default);
    }
}
