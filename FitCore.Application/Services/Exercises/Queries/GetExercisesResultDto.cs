using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Exercises.Queries
{
    public class GetExercisesResultDto
    {
        public List<GetExercisesDto> Exercises { get; set; }

        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public int PageSize { get; set; }
    }

    public class GetExercisesDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string EnglishName { get; set; }

        public string MuscleGroup { get; set; }

        public string EquipmentType { get; set; }

        public string DifficultyLevel { get; set; }

        public string ImagePath { get; set; }

        public bool IsActive { get; set; }
    }

    public class GetExercisesRequestDto
    {
        public string SearchKey { get; set; }

        public int? MuscleGroupId { get; set; }

        public int? EquipmentTypeId { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

    public class GetExercisesService : IGetExercisesService
    {
        private readonly IDataBaseContext _context;

        public GetExercisesService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<GetExercisesResultDto>> Execute(GetExercisesRequestDto request)
        {
            var exercises =
                _context.Exercises
                .Where(c => !c.IsRemoved)
                .Include(x => x.PrimaryMuscleGroup)
                .Include(x => x.EquipmentType)
                .Include(x => x.DifficultyLevel)
                .AsQueryable();

            //====================================
            // فیلتر جستجو
            //====================================

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                exercises =
                    exercises.Where(x =>
                        x.Name.Contains(request.SearchKey) ||
                        x.EnglishName.Contains(request.SearchKey) ||
                        x.PrimaryMuscleGroup.Name.Contains(request.SearchKey));
            }

            //====================================
            // فیلتر گروه عضلانی
            //====================================

            if (request.MuscleGroupId.HasValue)
            {
                exercises =
                    exercises.Where(x => x.PrimaryMuscleGroupId == request.MuscleGroupId.Value);
            }

            //====================================
            // فیلتر نوع تجهیزات
            //====================================

            if (request.EquipmentTypeId.HasValue)
            {
                exercises =
                    exercises.Where(x => x.EquipmentTypeId == request.EquipmentTypeId.Value);
            }

            int rowCount = await exercises.CountAsync();

            var result =
                await exercises
                .OrderByDescending(x => x.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new GetExercisesDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    EnglishName = x.EnglishName,
                    MuscleGroup = x.PrimaryMuscleGroup.Name,
                    EquipmentType = x.EquipmentType.Name,
                    DifficultyLevel = x.DifficultyLevel.Name,
                    ImagePath = x.ImagePath,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return new ResultDto<GetExercisesResultDto>
            {
                IsSuccess = true,
                Data = new GetExercisesResultDto
                {
                    Exercises = result,
                    CurrentPage = request.Page,
                    RowCount = rowCount,
                    PageCount = (int)Math.Ceiling((double)rowCount / request.PageSize),
                    PageSize = request.PageSize
                }
            };
        }
    }
}