using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FitCore.Application.Services.TrainingPrograms.Commands.EditTrainingProgram
{
    public class EditTrainingProgramService : IEditTrainingProgramService
    {
        private readonly IDataBaseContext _context;

        public EditTrainingProgramService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<RequestEditTrainingProgramDto>> Execute(RequestEditTrainingProgramDto request)
        {
            try
            {
                //====================================
                // بررسی برنامه تمرینی
                //====================================

                var trainingProgram =
                    await _context.TrainingPrograms
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.Id);

                if (trainingProgram == null)
                {
                    return new ResultDto<RequestEditTrainingProgramDto>()
                    {
                        IsSuccess = false,
                        Message = "برنامه تمرینی یافت نشد",
                        Data = null
                    };
                }

                //====================================
                // بررسی تکراری نبودن عنوان برای این عضو
                //====================================

                bool isExist =
                    await _context.TrainingPrograms
                    .AnyAsync(x =>
                        x.Id != request.Id &&
                        x.MemberId == trainingProgram.MemberId &&
                        x.Title == request.Title);

                if (isExist)
                {
                    return new ResultDto<RequestEditTrainingProgramDto>()
                    {
                        IsSuccess = false,
                        Message = "برنامه تمرینی با این عنوان قبلا برای این عضو ثبت شده است",
                        Data = null
                    };
                }

                //====================================
                // ویرایش برنامه تمرینی
                //====================================

                trainingProgram.Title = request.Title;
                trainingProgram.Description = request.Description;
                trainingProgram.TrainingProgramTypeId = request.TrainingProgramTypeId;
                trainingProgram.TrainingGoalTypeId = request.TrainingGoalTypeId;
                trainingProgram.SessionsPerWeek = request.SessionsPerWeek;
                trainingProgram.GymId = request.GymId;
                trainingProgram.StartDate = request.StartDate;
                trainingProgram.EndDate = request.EndDate;
                trainingProgram.IsActive = request.IsActive;
                trainingProgram.UpdateTime = DateTime.Now;

                await _context.SaveChangesAsync();

                //====================================
                // نتیجه
                //====================================

                return new ResultDto<RequestEditTrainingProgramDto>()
                {
                    IsSuccess = true,
                    Message = "برنامه تمرینی با موفقیت ویرایش شد",
                    Data = new RequestEditTrainingProgramDto
                    {
                        Id = trainingProgram.Id
                    }
                };
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message;
                return new ResultDto<RequestEditTrainingProgramDto>
                {
                    IsSuccess = false,
                    Message = "خطا در ویرایش برنامه تمرینی: " + inner,
                    Data = null
                };
            }
        }
    }

    public class RequestEditTrainingProgramDto
    {
        [Required]
        public long Id { get; set; }

        public long? GymId { get; set; }

        [DisplayName("نوع برنامه تمرینی")]
        [Required(ErrorMessage = "نوع برنامه تمرینی الزامی است")]
        public int TrainingProgramTypeId { get; set; }

        [DisplayName("هدف تمرینی")]
        [Required(ErrorMessage = "هدف تمرینی الزامی است")]
        public int TrainingGoalTypeId { get; set; }

        [DisplayName("تعداد جلسات در هفته")]
        public int? SessionsPerWeek { get; set; }

        [DisplayName("عنوان برنامه")]
        [Required(ErrorMessage = "عنوان برنامه الزامی است")]
        public string Title { get; set; }

        [DisplayName("توضیحات")]
        public string Description { get; set; }

        [DisplayName("تاریخ شروع")]
        [Required(ErrorMessage = "تاریخ شروع برنامه الزامی است")]
        public string StartDate { get; set; }

        [DisplayName("تاریخ پایان")]
        [Required(ErrorMessage = "تاریخ پایان برنامه الزامی است")]
        public string EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}