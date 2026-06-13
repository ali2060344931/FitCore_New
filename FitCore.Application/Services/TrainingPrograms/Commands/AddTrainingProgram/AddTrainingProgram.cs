using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.TrainingProgram;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FitCore.Application.Services.TrainingPrograms.Commands.AddTrainingProgram
{
    public class AddTrainingProgramService : IAddTrainingProgramService
    {
        private readonly IDataBaseContext _context;

        public AddTrainingProgramService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<RequestAddTrainingProgramDto>> Execute(RequestAddTrainingProgramDto request)
        {
            try
            {
                //====================================
                // بررسی عضو
                //====================================

                var member =
                    await _context.Members
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.MemberId);

                if (member == null)
                {
                    return new ResultDto<RequestAddTrainingProgramDto>()
                    {
                        IsSuccess = false,
                        Message = "عضو یافت نشد",
                        Data = null
                    };
                }

                //====================================
                // بررسی باشگاه (در صورت ارسال)
                //====================================

                if (request.GymId.HasValue)
                {
                    var gym =
                        await _context.Gyms
                        .FirstOrDefaultAsync(x =>
                            x.Id == request.GymId.Value);

                    if (gym == null)
                    {
                        return new ResultDto<RequestAddTrainingProgramDto>()
                        {
                            IsSuccess = false,
                            Message = "باشگاه یافت نشد",
                            Data = null
                        };
                    }
                }

                //====================================
                // بررسی تکراری نبودن عنوان برای این عضو
                //====================================

                bool isExist =
                    await _context.TrainingPrograms
                    .AnyAsync(x =>
                        x.MemberId == request.MemberId &&
                        x.Title == request.Title);

                if (isExist)
                {
                    return new ResultDto<RequestAddTrainingProgramDto>()
                    {
                        IsSuccess = false,
                        Message = "برنامه تمرینی با این عنوان قبلا برای این عضو ثبت شده است",
                        Data = null
                    };
                }

                //====================================
                // ساخت برنامه تمرینی
                //====================================

                TrainingProgram trainingProgram =
                    new TrainingProgram()
                    {
                        Title = request.Title,
                        Description = request.Description,
                        TrainingProgramTypeId = request.TrainingProgramTypeId,
                        TrainingGoalTypeId = request.TrainingGoalTypeId,
                        SessionsPerWeek = request.SessionsPerWeek,
                        GymId = request.GymId,
                        MemberId = request.MemberId,
                        StartDate = request.StartDate,
                        EndDate = request.EndDate,
                        IsActive = request.IsActive,
                        InsertTime = DateTime.Now,
                        CreatedByUserId = request.CreatedByUserId,
                    };

                //====================================
                // ذخیره در دیتابیس
                //====================================

                await _context.TrainingPrograms
                    .AddAsync(trainingProgram);

                await _context.SaveChangesAsync();

                //====================================
                // نتیجه
                //====================================

                return new ResultDto<RequestAddTrainingProgramDto>()
                {
                    IsSuccess = true,
                    Message = "برنامه تمرینی با موفقیت ثبت شد",
                    Data = new RequestAddTrainingProgramDto
                    {
                        Id = trainingProgram.Id
                    }
                };
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message;
                return new ResultDto<RequestAddTrainingProgramDto>
                {
                    IsSuccess = false,
                    Message = "خطا در ثبت برنامه تمرینی: " + inner,
                    Data = null
                };
            }
        }
    }

    public class RequestAddTrainingProgramDto
    {
        public long Id { get; set; }

        public long? GymId { get; set; }

        [Required(ErrorMessage = "عضو الزامی است")]
        public long MemberId { get; set; }

        public long CreatedByUserId { get; set; }

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