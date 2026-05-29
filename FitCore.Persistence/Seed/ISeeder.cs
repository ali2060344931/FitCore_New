using System;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{
    public interface ISeeder
    {
        Task SeedAsync(IServiceProvider serviceProvider);
    }
}
