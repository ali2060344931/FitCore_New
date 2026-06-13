using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Exercises.Commands.EditExercise
{
    public class EditExerciseService : IEditExerciseService
    {
        private readonly IDataBaseContext _context;

        public EditExerciseService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<RequestEditExerciseDto>> Execute(RequestEditExerciseDto request)
        {
            try
            {
                var exercise =
                    await _context.Exercises
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.Id && !x.IsRemoved);

                if (exercise == null)
                {
                    return new ResultDto<RequestEditExerciseDto>()
                    {
                        IsSuccess = false,
                        Message = "حرکت تمرینی یافت نشد",
                        Data = null
                    };
                }

                //====================================
                // بررسی تکراری نبودن نام حرکت
                //====================================

                bool isExist =
                    await _context.Exercises
                    .AnyAsync(x =>
                        x.Id != request.Id &&
                        x.Name == request.Name && !x.IsRemoved);

                if (isExist)
                {
                    return new ResultDto<RequestEditExerciseDto>()
                    {
                        IsSuccess = false,
                        Message = "حرکتی با این نام قبلا ثبت شده است",
                        Data = null
                    };
                }

                exercise.Name = request.Name;
                exercise.EnglishName = request.EnglishName;
                exercise.Description = request.Description;
                exercise.PrimaryMuscleGroupId = request.PrimaryMuscleGroupId;
                exercise.EquipmentTypeId = request.EquipmentTypeId;
                exercise.DifficultyLevelId = request.DifficultyLevelId;
                exercise.VideoUrl = request.VideoUrl;

                if (!string.IsNullOrWhiteSpace(request.ImagePath))
                {
                    exercise.ImagePath = request.ImagePath;
                }

                exercise.IsActive = request.IsActive;
                exercise.UpdateTime = DateTime.Now;

                await _context.SaveChangesAsync();

                return new ResultDto<RequestEditExerciseDto>()
                {
                    IsSuccess = true,
                    Message = "حرکت تمرینی با موفقیت ویرایش شد",
                    Data = new RequestEditExerciseDto
                    {
                        Id = exercise.Id
                    }
                };
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message;
                return new ResultDto<RequestEditExerciseDto>
                {
                    IsSuccess = false,
                    Message = "خطا در ویرایش حرکت تمرینی: " + inner,
                    Data = null
                };
            }
        }
    }

    public class RequestEditExerciseDto
    {
        [Required]
        public long Id { get; set; }

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

        public bool IsActive { get; set; }
    }
}