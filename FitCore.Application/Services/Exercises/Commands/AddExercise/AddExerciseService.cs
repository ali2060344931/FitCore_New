using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.TrainingProgram;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Exercises.Commands.AddExercise
{
    public class AddExerciseService : IAddExerciseService
    {
        private readonly IDataBaseContext _context;

        public AddExerciseService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<RequestAddExerciseDto>> Execute(RequestAddExerciseDto request)
        {
            try
            {
                //====================================
                // بررسی تکراری نبودن نام حرکت (در همان باشگاه یا سراسری)
                //====================================

                bool isExist =
                    await _context.Exercises
                    .AnyAsync(x =>
                        x.Name == request.Name &&
                        x.GymId == request.GymId &&
                        !x.IsRemoved);

                if (isExist)
                {
                    return new ResultDto<RequestAddExerciseDto>()
                    {
                        IsSuccess = false,
                        Message = "حرکتی با این نام قبلا ثبت شده است",
                        Data = null
                    };
                }

                //====================================
                // ساخت حرکت
                //====================================

                Exercise exercise =
                    new Exercise()
                    {
                        GymId = request.GymId,
                        Name = request.Name,
                        EnglishName = request.EnglishName,
                        Description = request.Description,
                        PrimaryMuscleGroupId = request.PrimaryMuscleGroupId,
                        EquipmentTypeId = request.EquipmentTypeId,
                        DifficultyLevelId = request.DifficultyLevelId,
                        VideoUrl = request.VideoUrl,
                        ImagePath = request.ImagePath,
                        IsActive = request.IsActive,
                        InsertTime = DateTime.Now,
                    };

                //====================================
                // ذخیره در دیتابیس
                //====================================

                await _context.Exercises
                    .AddAsync(exercise);

                await _context.SaveChangesAsync();

                //====================================
                // نتیجه
                //====================================

                return new ResultDto<RequestAddExerciseDto>()
                {
                    IsSuccess = true,
                    Message = "حرکت تمرینی با موفقیت ثبت شد",
                    Data = new RequestAddExerciseDto
                    {
                        Id = exercise.Id
                    }
                };
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message;
                return new ResultDto<RequestAddExerciseDto>
                {
                    IsSuccess = false,
                    Message = "خطا در ثبت حرکت تمرینی: " + inner,
                    Data = null
                };
            }
        }
    }

    public class RequestAddExerciseDto
    {
        public long Id { get; set; }

        /// <summary>
        /// شناسه باشگاه. مقدار null یعنی حرکت سراسری (مشترک) است
        /// و فقط توسط مدیر کل (SuperAdmin) قابل ثبت است.
        /// </summary>
        public long? GymId { get; set; }

        [DisplayName("نام حرکت")]
        [Required(ErrorMessage = "نام حرکت الزامی است")]
        public string Name { get; set; }

        [DisplayName("نام انگلیسی")]
        public string EnglishName { get; set; }

        [DisplayName("توضیحات / نحوه اجرا")]
        public string Description { get; set; }

        [DisplayName("گروه عضلانی اصلی")]
        [Required(ErrorMessage = "گروه عضلانی الزامی است")]
        public int PrimaryMuscleGroupId { get; set; }

        [DisplayName("نوع تجهیزات")]
        [Required(ErrorMessage = "نوع تجهیزات الزامی است")]
        public int EquipmentTypeId { get; set; }

        [DisplayName("سطح دشواری")]
        [Required(ErrorMessage = "سطح دشواری الزامی است")]
        public int DifficultyLevelId { get; set; }

        [DisplayName("آدرس ویدیو")]
        public string VideoUrl { get; set; }

        [DisplayName("تصویر")]
        public string ImagePath { get; set; }

        public bool IsActive { get; set; } = true;
    }
}