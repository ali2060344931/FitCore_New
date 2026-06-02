using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Foods.Commands.CreateFood
{
    public class CreateFoodDto
    {
        public string Title { get; set; } = string.Empty;
        public string EnglishTitle { get; set; } = string.Empty;
        public int CategoryTypeId { get; set; }
        public decimal CaloriesPerUnit { get; set; }
        public decimal ProteinPerUnit { get; set; }
        public decimal CarbohydratePerUnit { get; set; }
        public decimal FatPerUnit { get; set; }
        public int DefaultUnitId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
