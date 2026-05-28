using FitCore.Application.Contexts;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Provinces;
using FitCore.Domain.Entities.Setings;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System.Threading;
using System.Threading.Tasks;

namespace FitCore.Persistence.Contexts
{
    public class DataBaseContext :
        IdentityDbContext<AppUser, IdentityRole<long>, long>,
        IDataBaseContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        public DbSet<Gyms> Gyms { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<Setings> Setings { get; set; }
        public DbSet<UserOtpCode> UserOtpCodes { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplyQueryFilter(modelBuilder);
        }

        private void ApplyQueryFilter(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gyms>().HasQueryFilter(x => !x.IsRemoved);

            modelBuilder.Entity<Member>().HasQueryFilter(x => !x.IsRemoved);

            //modelBuilder.Entity<Member>()
            //    .HasOne(x => x.AppUser)
            //    .WithMany(x => x.Id)
            //    .HasForeignKey(x => x.);
        }


        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
