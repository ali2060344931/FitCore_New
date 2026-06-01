using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.INutritionProgram;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionPrograms.Commands.AddNutritionProgram
{
    public class AddNutritionProgramService :        IAddNutritionProgramService
    {
        private readonly IDataBaseContext _context;

        public AddNutritionProgramService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<RequestAddNutritionProgramDto>> Execute(RequestAddNutritionProgramDto request)
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
                    return new ResultDto<RequestAddNutritionProgramDto>()
                    {
                        IsSuccess = false,
                        Message = "باشگاه یافت نشد",
                        Data=null
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
                    return new ResultDto<RequestAddNutritionProgramDto>()
                    {
                        IsSuccess = false,
                        Message = "عضو یافت نشد",
                        Data = null
                    };
                }

                //====================================
                // بررسی تکراری نبودن عنوان
                //====================================

                bool isExist =
                    await _context.NutritionPrograms
                    .AnyAsync(x =>
                        x.GoalTypeId == request.GoalTypeId &&
                        x.ProgramTypeId == request.ProgramTypeId &&
                        x.MemberId == request.MemberId);

                if (isExist)
                {
                    return new ResultDto<RequestAddNutritionProgramDto>()
                    {
                        IsSuccess = false,
                        Message = "برنامه غذایی با این عنوان و برنامه زمانبندی قبلا ثبت شده است",
                        Data=null
                    };
                }

                //====================================
                // ساخت برنامه غذایی
                //====================================

                NutritionProgram nutritionProgram =
                    new NutritionProgram()
                    {
                        //Title = request.Title,
                        Description = request.Description,
                        GoalTypeId = request.GoalTypeId,
                        ProgramTypeId = request.ProgramTypeId,
                        GymId = request.GymId,
                        MemberId = request.MemberId,
                        StartDate = request.StartDate,
                        EndDate = request.EndDate,
                        IsActive = request.IsActive,
                        InsertTime = DateTime.Now,
                        CreatedByUserId = request.CreatedByUserId,
                        //CreateDate= DateTime.Now
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

                return new ResultDto<RequestAddNutritionProgramDto>()
                {
                    IsSuccess = true,
                    Message = "برنامه غذایی با موفقیت ثبت شد",
                    Data = new RequestAddNutritionProgramDto
                    {
                        Id = nutritionProgram.Id
                    }
                };
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message;
                return new ResultDto<RequestAddNutritionProgramDto>
                {
                    IsSuccess = false,
                    Message = "خطا در ثبت برنامه غذایی: " + inner,
                    Data = null
                    
                };
            }
        }
    }

    public class RequestAddNutritionProgramDto
    {
        public long Id { get; set; }
        public long GymId { get; set; }

        public long MemberId { get; set; }

        public long CreatedByUserId { get; set; }

        public int ProgramTypeId { get; set; }


        public int GoalTypeId { get; set; }


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