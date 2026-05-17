using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{
    public interface ISeeder
    {
        Task SeedAsync(IServiceProvider serviceProvider);
    }


}
