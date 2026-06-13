using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.TrainingProgramBuilder.Commands.ReorderTrainingExercises
{
    public interface IReorderTrainingExercisesService
    {
        Task<ResultDto> Execute(List<ReorderItemDto> items);
    }

    public class ReorderItemDto
    {
        public long Id { get; set; }
        public int SortOrder { get; set; }
    }

    /// <summary>
    /// تغییر ترتیب نمایش حرکات یک روز تمرینی (بدون تغییر سایر مشخصات)
    /// مورد استفاده در Drag &amp; Drop صفحه برنامه‌ساز
    /// </summary>
    public class ReorderTrainingExercisesService : IReorderTrainingExercisesService
    {
        private readonly IDataBaseContext _context;

        public ReorderTrainingExercisesService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(List<ReorderItemDto> items)
        {
            if (items == null || !items.Any())
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "آیتمی برای بازچینی ارسال نشده است"
                };
            }

            var ids = items.Select(x => x.Id).ToList();

            var exerciseItems =
                await _context.TrainingExerciseItems
                .Where(x => ids.Contains(x.Id) && !x.IsRemoved)
                .ToListAsync();

            foreach (var item in items)
            {
                var entity = exerciseItems.FirstOrDefault(x => x.Id == item.Id);

                if (entity != null)
                {
                    entity.SortOrder = item.SortOrder;
                    entity.UpdateTime = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "ترتیب با موفقیت ذخیره شد"
            };
        }
    }
}