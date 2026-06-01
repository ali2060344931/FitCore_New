using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

namespace FitCore.Application.Services.NutritionProgramBuilder.Queries
{
    public interface IGetProgramBuilderService
    {
        ResultDto<ProgramBuilderDto> Execute(long programId);
    }
    public class GetProgramBuilderService :
    IGetProgramBuilderService
    {
        private readonly IDataBaseContext _context;

        public GetProgramBuilderService(
            IDataBaseContext context)
        {
            _context = context;
        }



        public ResultDto<ProgramBuilderDto> Execute(long programId)
        {
            var program =
                _context.NutritionPrograms
                    .Include(x => x.Member)
                        .ThenInclude(x => x.AppUser)

                    .Include(x => x.ProgramType)

                    .Include(x => x.GoalType)

                    .Include(x => x.Days)
                        .ThenInclude(x => x.Meals)
                            .ThenInclude(x => x.Items)
                                .ThenInclude(x => x.Food)

                    .FirstOrDefault(x => x.Id == programId);

            if (program == null)
            {
                return null;
            }


            var result = new ProgramBuilderDto
            {

                ProgramId = program.Id,


                MemberName =
                    program.Member.AppUser.FullName + " " + program.Member.AppUser.PhoneNumber,

                ProgramType = program.ProgramType.Name,
                GoalType = program.GoalType.Name,

                StartDate = program.StartDate,

                EndDate = program.EndDate,

                Days = program.Days
                    .OrderBy(x => x.DayNumber)
                    .Select(day =>
                        new ProgramDayDto
                        {
                            Id = day.Id,

                            Title = day.Title,

                            DayNumber = day.DayNumber,

                            Meals = day.Meals
                                .OrderBy(x => x.SortOrder)
                                .Select(meal =>
                                    new ProgramMealDto
                                    {
                                        MealId = meal.Id,

                                        Title = meal.Title,

                                        MealType =
                                            meal.MealType.ToString(),

                                        Foods =
                                            meal.Items
                                            .Select(item =>
                                                new ProgramMealItemDto
                                                {
                                                    MealItemId =
                                                        item.Id,

                                                    FoodName =
                                                        item.Food.Title,

                                                    Amount =
                                                        item.Amount,

                                                    Unit =
                                                        item.UnitType
                                                        .ToString()
                                                })
                                            .ToList()
                                    })
                                .ToList()
                        })
                    .ToList()
            };

            return new ResultDto<ProgramBuilderDto>
            {
                IsSuccess = true,
                Data = result
            };
        }

    }
    public class ProgramBuilderDto
    {
        public long ProgramId { get; set; }

        public string GoalType { get; set; }

        public string MemberName { get; set; }

        public string ProgramType { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public List<ProgramDayDto> Days { get; set; }
    }


    public class ProgramDayDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public int DayNumber { get; set; }

        public List<ProgramMealDto> Meals { get; set; }
    }


    public class ProgramMealDto
    {
        public long MealId { get; set; }

        public string Title { get; set; }

        public string MealType { get; set; }

        public List<ProgramMealItemDto> Foods { get; set; }
    }


    public class ProgramMealItemDto
    {
        public long MealItemId { get; set; }

        public string FoodName { get; set; }

        public decimal Amount { get; set; }

        public string Unit { get; set; }
    }
}
