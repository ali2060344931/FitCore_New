using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Interfaces.IGym;
using FitCore.Application.Interfaces.IMembers;
using FitCore.Application.Interfaces.ISms;
using FitCore.Application.Services.Auth;
using FitCore.Application.Services.Facads;
using FitCore.Application.Services.Gyms.Commands;
using FitCore.Application.Services.Gyms.Commands.AddGym;
using FitCore.Application.Services.Gyms.Commands.DeleteGym;
using FitCore.Application.Services.Member.Queries;
using FitCore.Application.Services.Members.Queries;
using FitCore.Application.Services.Provinces.Queries;
using FitCore.Application.Services.Setings.Queries.GetSetings;
using FitCore.Application.Services.SiteSettings;
using FitCore.Application.Services.SmsService.Commands;
using FitCore.Application.Services.Users.Commands.LoginUser;
using FitCore.Application.Services.Users.Commands.LogoutUser;
using FitCore.Common.Roles;
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

using SendOtpService =
    FitCore.Application.Services.Auth.SendOtpService;

using VerifyOtpService =
    FitCore.Application.Services.Auth.VerifyOtpService;

var builder = WebApplication.CreateBuilder(args);

#region Database

string connectionString =
    @"Data Source=.;Initial Catalog=FitCoreDb;Integrated Security=True;TrustServerCertificate=True";

builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();

#endregion

#region Identity

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddIdentity<AppUser, IdentityRole<long>>(options =>
    {
        options.Password.RequireDigit = true;

        options.Password.RequiredLength = 6;

        options.Password.RequireUppercase = true;

        options.Password.RequireLowercase = true;

        options.Password.RequireNonAlphanumeric = true;

        options.User.RequireUniqueEmail = false;

        options.SignIn.RequireConfirmedPhoneNumber = false;
    })

    .AddEntityFrameworkStores<DataBaseContext>()

    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";

    options.AccessDeniedPath =
        "/Auth/AccessDenied";

    options.ExpireTimeSpan =
        TimeSpan.FromDays(30);

    options.SlidingExpiration = true;

    options.Cookie.HttpOnly = true;
});

#endregion

#region FluentValidation

builder.Services
    .AddValidatorsFromAssemblyContaining<RegisterUserValidator>();

builder.Services
    .AddFluentValidationAutoValidation();

#endregion

#region Dependency Injection


//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("ManagementAccess", policy => policy.RequireRole(UserRoles.SuperAdmin, UserRoles.Admin));

//    options.AddPolicy("FinancialAccess", policy => policy.RequireRole(UserRoles.SuperAdmin, UserRoles.Admin));

//    options.AddPolicy("GymOwnerOnly", policy => policy.RequireRole(UserRoles.SuperAdmin, UserRoles.Admin));
//});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ElevatedRights", policy =>        policy.RequireRole("Admin", "SuperAdmin"));

    options.AddPolicy("FinancialManager", policy =>        policy.RequireRole("Admin", "SuperAdmin", "Accountant"));
});


// ===== Common =====

builder.Services.AddScoped<IGetSetings, GetSetingService>();

builder.Services.AddScoped<ISiteSettingService, SiteSettingService>();

builder.Services.AddScoped<ILoginUserService, LoginUserService>();

builder.Services.AddScoped<ILogoutUserService, LogoutUserService>();

builder.Services.AddScoped<ISmsService, SmsService>();

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<ILoginTokenStore,
    MemoryLoginTokenStore>();

// ===== Gym =====

builder.Services.AddScoped<IAddGymService,
    AddGymService>();

builder.Services.AddScoped<IDeleteGymService,
    DeleteGymService>();

builder.Services.AddScoped<IEditGymService,
    EditGymService>();

builder.Services.AddScoped<IGetGymByIdService,
    GetGymByIdService>();

builder.Services.AddScoped<IGetGymsService,
    GetGymsService>();

builder.Services.AddScoped<ICompleteGymInfoService,
    CompleteGymInfoService>();

builder.Services.AddScoped<IGetProvincesService,
    GetProvincesService>();

builder.Services.AddScoped<IGetCitiesService,
    GetCitiesService>();

// ===== Member =====

builder.Services.AddScoped<IGetMembersByIdService,
    GetMembersByIdService>();


builder.Services.AddScoped<IMemberFacad, MemberFacad>();
builder.Services.AddScoped<IGetMembersService, GetMembersService>();


// ===== Auth =====

builder.Services.AddScoped<RegisterUserService>();

builder.Services.AddScoped<SendOtpService>();

builder.Services.AddScoped<VerifyOtpService>();

builder.Services.AddScoped<
    IUserClaimsPrincipalFactory<AppUser>,
    CustomClaimsPrincipalFactory>();

#endregion

#region MVC

builder.Services.AddControllersWithViews();

#endregion

var app = builder.Build();

#region Seeder

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await SeederRunner.RunAsync(services);
}

#endregion

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
    pattern:
    "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern:
    "{controller=Home}/{action=Index}/{id?}");

#endregion

#region No Cache

app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] =
        "no-cache, no-store, must-revalidate";

    context.Response.Headers["Pragma"] =
        "no-cache";

    context.Response.Headers["Expires"] =
        "0";

    await next();
});

#endregion

app.Run();