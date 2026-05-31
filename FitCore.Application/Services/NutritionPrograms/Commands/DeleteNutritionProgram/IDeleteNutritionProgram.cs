using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.IGym;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionPrograms.Commands.DeleteNutritionProgram
{
    public interface IDeleteNutritionProgramService
    {
        ResultDto Execute(long Id);
    }


    public class DeleteNutritionProgramServises : IDeleteNutritionProgramService
    {

        private readonly IDataBaseContext _context;

        public DeleteNutritionProgramServises(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long Id)
        {
            var prog = _context.NutritionPrograms.Find(Id);
            if (prog == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "برنامه ای یافت نشد"
                };
            }
            prog.RemoveTime = DateTime.Now;
            prog.IsRemoved = true;
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "برنامه غذایی با موفقیت حذف شد"
            };
        }



    }

}
