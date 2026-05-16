using FitCore.Application.Interfaces.Contexts;
using FitCore.Common.Roles;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Products;
using FitCore.Domain.Entities.Setings;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using System;

namespace FitCore.Persistence.Contexts
{
    public class DataBaseContext : DbContext, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)

        //public class DatabaseContext :
        //        IdentityDbContext<AppUser, IdentityRole<long>, long>

        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<ProductFeatures> ProductFeatures { get; set; }
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Setings> Setings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Seed Data
            SeedData(modelBuilder);


            // اعمال ایندکس بر روی فیلد ایمیل
            // اعمال عدم تکراری بودن ایمیل
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            //-- عدم نمایش اطلاعات حذف شده
            ApplyQueryFilter(modelBuilder);

        }

        private void ApplyQueryFilter(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Role>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<UserInRole>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Category>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Gym>().HasQueryFilter(x => !x.IsRemoved);
            modelBuilder.Entity<Member>().HasQueryFilter(x => !x.IsRemoved);
            modelBuilder.Entity<Member>().HasOne(x => x.Gym).WithMany().HasForeignKey(x => x.GymId);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = nameof(UserRoles.Admin) });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 2, Name = nameof(UserRoles.Operator) });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 3, Name = nameof(UserRoles.Customer) });
            modelBuilder.Entity<Setings>().HasData(new Setings { Id = 1, Code = "01" ,TextFilde="نرم افزار فیتکو",Email="Ali@Gmail.com",Phone="09111161996"});
        }

    }


}