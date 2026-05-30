using FitCore.Application.Interfaces.INutritionProgram;
using FitCore.Application.Services.NutritionPrograms.Commands.AddNutritionProgram;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionPrograms.Queries.GetMemberPrograms
{
    public class GetMemberProgramService: IGetNutritionProgramsService
    {
        private readonly IGetNutritionProgramsService _getNutritionProgramsService;

        public GetMemberProgramService(IGetNutritionProgramsService getNutritionProgramsService)
        {
            _getNutritionProgramsService = getNutritionProgramsService;
        }

     public  Task<ResultDto<NutritionProgramDto>> Execute()
        {

            return new ResultDto<NutritionProgramDto>
            {
                IsSuccess = true,

            }
        }
    }
}
