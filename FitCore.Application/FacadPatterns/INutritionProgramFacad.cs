using FitCore.Application.Interfaces.INutritionProgram;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.FacadPatterns
{
    public class INutritionProgramFacad
    {
        IAddNutritionProgramService addNutritionProgramService { get; }
    }
}
