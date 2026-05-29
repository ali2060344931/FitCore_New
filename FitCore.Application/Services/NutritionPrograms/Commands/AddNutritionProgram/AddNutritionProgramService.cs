using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.INutritionProgram;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;

using Microsoft.EntityFrameworkCore;

using System;

using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionPrograms.Commands.AddNutritionProgram
{
    public class AddNutritionProgramService :
        IAddNutritionProgramService
    {
        private readonly IDataBaseContext _context;

        public AddNutritionProgramService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(
            RequestAddNutritionProgramDto request)
        {
            try
            {
                //====================================
                // بررسی باشگاه
                //====================================

                var gym =
                    await _context.Gyms
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.GymId);

                if (gym == null)
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "باشگاه یافت نشد"
                    };
                }

                //====================================
                // بررسی عضو
                //====================================

                var member =
                    await _context.Members
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.MemberId);

                if (member == null)
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "عضو یافت نشد"
                    };
                }

                //====================================
                // بررسی تکراری نبودن عنوان
                //====================================

                bool isExist =
                    await _context.NutritionPrograms
                    .AnyAsync(x =>
                        x.Title == request.Title &&
                        x.MemberId == request.MemberId);

                if (isExist)
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "برنامه غذایی با این عنوان قبلا ثبت شده است"
                    };
                }

                //====================================
                // ساخت برنامه غذایی
                //====================================

                NutritionProgram nutritionProgram =
                    new NutritionProgram()
                    {
                        Title = request.Title,
                        Description = request.Description,
                        GoalType = request.Goal,
                        GymId = request.GymId,
                        MemberId = request.MemberId,
                        StartDate = request.StartDate,
                        EndDate = request.EndDate,
                        IsActive = request.IsActive,
                        InsertTime = DateTime.Now,
                    };

                //====================================
                // ذخیره در دیتابیس
                //====================================

                await _context.NutritionPrograms
                    .AddAsync(nutritionProgram);

                await _context.SaveChangesAsync();

                //====================================
                // نتیجه
                //====================================

                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "برنامه غذایی با موفقیت ثبت شد"
                };
            }
            catch (Exception)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "خطا در ثبت برنامه غذایی"
                };
            }
        }
    }

    public class RequestAddNutritionProgramDto
    {
        public long GymId { get; set; }

        public long MemberId { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public GoalType Goal { get; set; }

        public int Calories { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}