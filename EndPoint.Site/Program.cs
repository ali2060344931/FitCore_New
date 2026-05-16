using FitCore.Application.Interfaces.Contexts;
using FitCore.Application.Interfaces.FacadPatterns;
using FitCore.Application.Services.Products.FacadPattern;
using FitCore.Application.Services.Setings.Queries.GetSetings;
using FitCore.Application.Services.SiteSettings;
using FitCore.Application.Services.Users.Commands.EditUser;
using FitCore.Application.Services.Users.Commands.RemoveUser;
using FitCore.Application.Services.Users.Commands.RgegisterUser;
using FitCore.Application.Services.Users.Commands.UserLogin;
using FitCore.Application.Services.Users.Commands.UserSatusChange;
using FitCore.Application.Services.Users.Queries.GetRoles;
using FitCore.Application.Services.Users.Queries.GetUsers;
using FitCore.Domain.Entities.Users;
using FitCore.Persistence.Contexts;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;

using static FitCore.Persistence.Contexts.DataBaseContext;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = new PathString("/");
    options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
});

//builder.Services.AddIdentity<AppUser, IdentityRole<long>>()
//    .AddEntityFrameworkStores<DatabaseContext>()
//    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";

    options.AccessDeniedPath = "/Auth/AccessDenied";
});

builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();




builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
builder.Services.AddScoped<IGetUsersService, GetUsersService>();
builder.Services.AddScoped<IGetRolesService, GetRolesService>();
builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();
builder.Services.AddScoped<IRemoveUserService, RemoveUserService>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();
builder.Services.AddScoped<IUserSatusChangeService, UserSatusChangeService>();
builder.Services.AddScoped<IEditUserService, EditUserService>();
builder.Services.AddScoped<IGetSetings, GetSetingService>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ISiteSettingService, SiteSettingService>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();



//FacadeInjection
builder.Services.AddScoped<IProductFacad, ProductFacad>();


builder.Services.AddControllersWithViews();


builder.Services.AddFluentValidationAutoValidation();


//var app = builder.Build();

string connectionString =
@"Data Source=.;Initial Catalog=FitCoreDb;Integrated Security=True;TrustServerCertificate=True";

builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseSqlServer(connectionString));


#endregion

var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

#endregion

app.Run();
