using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace FitCore.Application.Services.TrainingPrograms.Commands.DeleteTrainingProgram
{
    public class DeleteTrainingProgramService : IDeleteTrainingProgramService
    {
        private readonly IDataBaseContext _context;

        public DeleteTrainingProgramService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(long trainingProgramId)
        {
            var trainingProgram =
                await _context.TrainingPrograms
                .FirstOrDefaultAsync(x => x.Id == trainingProgramId);

            if (trainingProgram == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "برنامه تمرینی یافت نشد"
                };
            }

            trainingProgram.RemoveTime = DateTime.Now;
            trainingProgram.IsRemoved = true;

            await _context.SaveChangesAsync();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "برنامه تمرینی با موفقیت حذف شد"
            };
        }
    }
}