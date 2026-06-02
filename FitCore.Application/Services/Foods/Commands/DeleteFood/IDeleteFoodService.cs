using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Foods.Commands.DeleteFood
{
    public interface IDeleteFoodService
    {
        Task<FitCore.Common.Dto.ResultDto> Execute(long id);
    }
    public class DeleteFoodService : IDeleteFoodService
    {
        private readonly IDataBaseContext _context;

        public DeleteFoodService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(long id)
        {
            var food = await _context.Foods.FirstOrDefaultAsync(x => x.Id == id);
            if (food == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "غذا یافت نشد."
                };
            }

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "غذا با موفقیت حذف شد."
            };
        }
    }
}
