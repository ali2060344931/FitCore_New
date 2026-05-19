using FitCore.Application.Interfaces.Contexts;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Setings;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

        public DbSet<Gym> Gyms { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<Setings> Setings { get; set; }
        public DbSet<UserOtpCode> UserOtpCodes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedData(modelBuilder);
            ApplyQueryFilter(modelBuilder);
        }

        private void ApplyQueryFilter(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gym>().HasQueryFilter(x => !x.IsRemoved);

            modelBuilder.Entity<Member>().HasQueryFilter(x => !x.IsRemoved);

            modelBuilder.Entity<Member>()
                .HasOne(x => x.Gym)
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.GymId);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {





            //modelBuilder.Entity<Setings>().HasData(new Setings
            //{
            //    Id = 1,
            //    Code = "01",
            //    TextFilde = "نرم افزار فیتکو",
            //    Email = "Ali@Gmail.com",
            //    Phone = "09111161996"
            //});
        }
    }
}
