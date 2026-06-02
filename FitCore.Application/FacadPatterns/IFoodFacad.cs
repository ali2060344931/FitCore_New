using FitCore.Application.Services.Foods.Commands.CreateFood;
using FitCore.Application.Services.Foods.Commands.DeleteFood;
using FitCore.Application.Services.Foods.Commands.EditFood;
using FitCore.Application.Services.Foods.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.FacadPatterns
{
    public interface IFoodFacad
    {
        IAddFoodService AddFoodService { get; }
        IEditFoodService EditFoodService { get; }
        IDeleteFoodService DeleteFoodService { get; }
        IGetFoodsService GetFoodsService { get; }
        IGetFoodByIdService GetFoodByIdService { get; }
    }
}
