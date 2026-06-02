using FitCore.Application.Contexts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Foods.Queries
{
    public interface IFoodService
    {
        long GetDefaultUnitId(long foodId);
    }
    public class FoodService : IFoodService
    {
        private readonly IDataBaseContext _context;

        public FoodService(IDataBaseContext context)
        {
            _context = context;
        }

        public long GetDefaultUnitId(long foodId)
        {
            // پیدا کردن غذا و دریافت واحد پیش‌فرض آن
            var food = _context.Foods.FirstOrDefault(f => f.Id == foodId);

            // اگر غذا یافت نشد یا واحدی نداشت، 0 برگردان
            return food?.DefaultUnitId ?? 0;
        }
    }
}
