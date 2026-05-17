using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Setings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


using System.Threading;
using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.Contexts
{
    public interface IDataBaseContext
    {
        DbSet<Gym> Gyms { get; set; }

        DbSet<Member> Members { get; set; }

        DbSet<Setings> Setings { get; set; }
        DatabaseFacade Database { get; }
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
