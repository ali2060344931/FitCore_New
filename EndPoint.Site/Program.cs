using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Interfaces.IGym;
using FitCore.Application.Interfaces.ISms;
using FitCore.Application.Services.Auth;
using FitCore.Application.Services.Facads;
using FitCore.Application.Services.Gyms.Commands;
using FitCore.Application.Services.Gyms.Commands.DeleteGym;
using FitCore.Application.Services.Setings.Queries.GetSetings;
using FitCore.Application.Services.SiteSettings;
using FitCore.Application.Services.SmsService.Commands;
using FitCore.Application.Services.Users.Commands.LoginUser;
using FitCore.Application.Services.Users.Commands.LogoutUser;
using FitCore.Domain.Entities.Users;
using FitCore.Persistence.Contexts;
using FitCore.Persistence.Seed;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;


//using static FitCore.Application.Services.Users.Commands.RegisterUser.RegisterUserService;

using SendOtpService = FitCore.Application.Services.Auth.SendOtpService;
using VerifyOtpService = FitCore.Application.Services.Auth.VerifyOtpService;
//using RegisterUserService = FitCore.Application.Services.Auth.RegisterUserService;

var builder = WebApplication.CreateBuilder(args);

#region Database

string connectionString =
    @"Data Source=.;Initial Catalog=FitCoreDb;Integrated Security=True;TrustServerCertificate=True";

builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseSqlServer(connectionString));

#endregion

#region Identity — ✅ اصلاح شده

// HttpContextAccessor مورد نیاز SignInManager
builder.Services.AddHttpContextAccessor();

// =====================================================
// ❌ حذف شد: builder.Services.AddAuthentication().AddCookie()
// ✅ فقط AddIdentity — خودش Authentication را تنظیم می‌کند
// =====================================================

builder.Services.AddIdentity<AppUser, IdentityRole<long>>(options =>
{
    // تنظیمات رمز عبور یکجا در اینجا
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;

    // تنظیمات اضافی Identity در صورت نیاز
    options.User.RequireUniqueEmail = false; // چون با موبایل کار می‌کنیم
    options.SignIn.RequireConfirmedPhoneNumber = true;
})

.AddEntityFrameworkStores<DataBaseContext>()
.AddDefaultTokenProviders();

// تنظیمات کوکی Identity — ✅ این روش درست است
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(30); // یا هر مدتی که می‌خواهید
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    //options.Cookie.SameSite = SameSiteMode.Lax;
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
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ILoginTokenStore, MemoryLoginTokenStore>();

//<=====Gym=====>
builder.Services.AddScoped<IAddGymService, AddGymService>();
builder.Services.AddScoped<IDeleteGymService, DeleteGymService>();
builder.Services.AddScoped<IEditGymService, EditGymService>();
builder.Services.AddScoped<IGetGymByIdService, GetGymByIdService>();
builder.Services.AddScoped<IGetGymsService, GetGymsService>();
//>=====Gym=====<



builder.Services.AddScoped<RegisterUserService>();
builder.Services.AddScoped<SendOtpService>();
builder.Services.AddScoped<VerifyOtpService>();

builder.Services.AddScoped<IMemberFacad, MemberFacad>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipalFactory>();

#endregion

#region MVC

builder.Services.AddControllersWithViews();

#endregion

var app = builder.Build();


//جهت ثبت اولیه مقادیر در برخی از جداول
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

// ✅ ترتیب صحیح: اول Authentication بعد Authorization
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

app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";

    await next();
});

app.Run();
