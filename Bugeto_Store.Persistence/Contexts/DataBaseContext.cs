using Bugeto_Store.Application.Interfaces.Contexts;
using Bugeto_Store.Common.Roles;
using Bugeto_Store.Domain.Entities.Products;
using Bugeto_Store.Domain.Entities.Users;

using Microsoft.EntityFrameworkCore;

using System;

namespace Bugeto_Store.Persistence.Contexts
{
    public class DataBaseContext : DbContext, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<ProductFeatures> ProductFeatures { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = nameof(UserRoles.Admin) });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 2, Name = nameof(UserRoles.Operator) });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 3, Name = nameof(UserRoles.Customer) });

            //اعمال ایندکس برای فیلد ایمیل
            //اعمال عدم تکراری بودن ایمل
            modelBuilder.Entity<User>().HasIndex(p=>p.Email).IsUnique();
            //جهت نمایش کاربران فعال
            modelBuilder.Entity<User>().HasQueryFilter(p=>!p.IsRemoved);



        }

}


}