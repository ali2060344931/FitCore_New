using FitCore.Application.Interfaces.Contexts;
using FitCore.Application.Interfaces.FacadPatterns;
using FitCore.Application.Services.Facads;
using FitCore.Application.Services.Setings.Queries.GetSetings;
using FitCore.Application.Services.SiteSettings;
using FitCore.Application.Services.Users.Commands.LoginUser;
using FitCore.Application.Services.Users.Commands.LogoutUser;
using FitCore.Application.Services.Users.Commands.RegisterUser;
using FitCore.Domain.Entities.Users;
using FitCore.Persistence.Contexts;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;

using static FitCore.Application.Services.Users.Commands.RegisterUser.RegisterUserService;

var builder = WebApplication.CreateBuilder(args);

#region Database

string connectionString =
@"Data Source=.;Initial Catalog=FitCoreDb;Integrated Security=True;TrustServerCertificate=True";

builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseSqlServer(connectionString));

#endregion

#region Identity

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
});




builder.Services.AddIdentity<AppUser, IdentityRole<long>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<DataBaseContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

#endregion

#region FluentValidation


builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();
builder.Services.AddFluentValidationAutoValidation();



#endregion

#region Dependency Injection

builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
builder.Services.AddScoped<IGetSetings, GetSetingService>();
builder.Services.AddScoped<ISiteSettingService, SiteSettingService>();
builder.Services.AddScoped<ILoginUserService, LoginUserService>();
builder.Services.AddScoped<ILogoutUserService, LogoutUserService>();
builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();

builder.Services.AddScoped<IMemberFacad, MemberFacad>();
builder.Services.AddMemoryCache();

#endregion

#region MVC

builder.Services.AddControllersWithViews();

#endregion

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await SeederRunner.RunAsync(services);
}


#region Middleware

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

#endregion

#region Routes

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

#endregion

app.Run();
