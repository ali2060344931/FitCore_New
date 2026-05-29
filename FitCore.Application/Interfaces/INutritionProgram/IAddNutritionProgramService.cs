using FitCore.Application.Services.NutritionPrograms.Commands.AddNutritionProgram;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.INutritionProgram
{
    public interface IAddNutritionProgramService
    {
        Task<ResultDto> Execute(RequestAddNutritionProgramDto request);
    }
}
