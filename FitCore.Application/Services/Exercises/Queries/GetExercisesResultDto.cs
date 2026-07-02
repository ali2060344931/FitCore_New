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

        /// <summary>
        /// آیا این حرکت سراسری (مشترک بین همه باشگاه‌ها) است؟
        /// </summary>
        public bool IsGlobal { get; set; }

        /// <summary>
        /// نام باشگاه صاحب حرکت (در صورتی که سراسری نباشد)
        /// </summary>
        public string GymName { get; set; }
    }

    public class GetExercisesRequestDto
    {
        public string SearchKey { get; set; }

        public int? MuscleGroupId { get; set; }

        public int? EquipmentTypeId { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        /// <summary>
        /// شناسه باشگاه کاربر جاری (مدیر باشگاه).
        /// اگر IsAdmin = true باشد، این مقدار نادیده گرفته می‌شود.
        /// </summary>
        public long? GymId { get; set; }

        /// <summary>
        /// آیا کاربر جاری مدیر کل (SuperAdmin) است؟
        /// در این صورت همه حرکات (همه باشگاه‌ها + سراسری) قابل مشاهده است.
        /// </summary>
        public bool IsAdmin { get; set; }


        /// <summary>
        /// فیلتر مالکیت: true = عمومی، false = متعلق به باشگاه، null = همه
        /// </summary>
        public bool? IsGlobalFilter { get; set; }
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
                .Include(x => x.Gym)
                .AsQueryable();

            // در متد Execute سرویس، بخش فیلترها را دقیقاً به این شکل تغییر دهید:

            //====================================
            // فیلتر جستجو (فقط نام فارسی)
            //====================================
            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                exercises =
                    exercises.Where(x => x.Name.Contains(request.SearchKey));
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

            //====================================
            // فیلتر مالکیت (جدید)
            //====================================
            if (request.IsGlobalFilter.HasValue)
            {
                if (request.IsGlobalFilter.Value)
                {
                    exercises = exercises.Where(x => x.GymId == null);
                }
                else
                {
                    exercises = exercises.Where(x => x.GymId != null);
                }
            }

            int rowCount = await exercises.CountAsync();

            var result =
                await exercises
                .OrderBy(x => x.Name)
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
                    IsActive = x.IsActive,
                    IsGlobal = x.GymId == null,
                    GymName = x.Gym != null ? x.Gym.Name : null
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