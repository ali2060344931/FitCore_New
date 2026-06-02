using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.Foods.Commands.CreateFood;
using FitCore.Application.Services.Foods.Commands.DeleteFood;
using FitCore.Application.Services.Foods.Commands.EditFood;
using FitCore.Application.Services.Foods.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Foods.FoodFacad
{
    public class FoodFacad : IFoodFacad
    {
        public FoodFacad(
            IAddFoodService addFoodService,
            IEditFoodService editFoodService,
            IDeleteFoodService deleteFoodService,
            IGetFoodsService getFoodsService,
            IGetFoodByIdService getFoodByIdService)
        {
            AddFoodService = addFoodService;
            EditFoodService = editFoodService;
            DeleteFoodService = deleteFoodService;
            GetFoodsService = getFoodsService;
            GetFoodByIdService = getFoodByIdService;
        }

        public IAddFoodService AddFoodService { get; }
        public IEditFoodService EditFoodService { get; }
        public IDeleteFoodService DeleteFoodService { get; }
        public IGetFoodsService GetFoodsService { get; }
        public IGetFoodByIdService GetFoodByIdService { get; }
    }
}
