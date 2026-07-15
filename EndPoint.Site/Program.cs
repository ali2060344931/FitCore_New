using EndPoint.Site.BaleBot.Handlers;

using FitCore.Application.Common.Options;
using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Interfaces.IGym;
using FitCore.Application.Interfaces.IMembers;
using FitCore.Application.Interfaces.ISms;
using FitCore.Application.Services;
using FitCore.Application.Services.AI;
using FitCore.Application.Services.Announcements.Commands.AddAnnouncement;
using FitCore.Application.Services.Announcements.Commands.DeleteAnnouncement;
using FitCore.Application.Services.Announcements.Commands.EditAnnouncement;
using FitCore.Application.Services.Announcements.Dashboard;
using FitCore.Application.Services.Announcements.Dashboard.DismissAnnouncement;
using FitCore.Application.Services.Announcements.Dashboard.GetDashboardAnnouncements;
using FitCore.Application.Services.Announcements.Dashboard.RegisterAnnouncementClick;
using FitCore.Application.Services.Announcements.Dashboard.RegisterAnnouncementView;
using FitCore.Application.Services.Announcements.Facade;
using FitCore.Application.Services.Announcements.Queries;
using FitCore.Application.Services.Announcements.Queries.GetAnnouncementById;
using FitCore.Application.Services.Announcements.Queries.GetAnnouncements;
using FitCore.Application.Services.Announcements.Queries.GetRoles;
using FitCore.Application.Services.Auth;
using FitCore.Application.Services.Dashboard;
using FitCore.Application.Services.Exercises.ExerciseFacad;
using FitCore.Application.Services.Facads;
using FitCore.Application.Services.Foods.Commands.CreateFood;
using FitCore.Application.Services.Foods.Commands.DeleteFood;
using FitCore.Application.Services.Foods.Commands.EditFood;
using FitCore.Application.Services.Foods.FoodFacad;
using FitCore.Application.Services.Foods.Queries;
using FitCore.Application.Services.Gyms.Commands;
using FitCore.Application.Services.Gyms.Commands.AddGym;
using FitCore.Application.Services.Gyms.Commands.DeleteGym;
using FitCore.Application.Services.Halpe;
using FitCore.Application.Services.Member.Queries;
using FitCore.Application.Services.Members.Commands;
using FitCore.Application.Services.Members.Queries;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.AddNutritionMealDto;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.AddNutritionMealItemDto;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.AddNutritionProgramDay;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.AutoGenerateNutritionDays;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.EditNutritionDay;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.EditNutritionMeal;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.EditNutritionMealItem;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.RemoveNutritionDay;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.RemoveNutritionMeal;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.RemoveNutritionMealItem;
using FitCore.Application.Services.NutritionProgramBuilder.Queries;
using FitCore.Application.Services.NutritionProgramReports.Queries;
using FitCore.Application.Services.NutritionPrograms.NutritionProgramsFacad;
using FitCore.Application.Services.Provinces.Queries;
using FitCore.Application.Services.Setings.Queries.GetSetings;
using FitCore.Application.Services.SiteSettings;
using FitCore.Application.Services.SmsService.Commands;
using FitCore.Application.Services.Tickets;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.RemoveAllTrainingDays;
using FitCore.Application.Services.TrainingProgramBuilder.TrainingProgramBuilderFacad;
using FitCore.Application.Services.TrainingPrograms.TrainingProgramsFacad;
using FitCore.Domain.Entities.Users;
using FitCore.Persistence.Contexts;

using FluentValidation.AspNetCore;

using GymBot.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

using System;
using System.IO;
using System.Threading.Tasks;

using SendOtpService =
    FitCore.Application.Services.Auth.SendOtpService;
using VerifyOtpService =
    FitCore.Application.Services.Auth.VerifyOtpService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region Database
string connectionString;
if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("DevelopmentConnection");
}
else
{
    connectionString = builder.Configuration.GetConnectionString("ProductionConnection");
}
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
#endregion




#region Identity

builder.Services.AddHttpContextAccessor();



//........................

builder.Services.AddHttpClient("ClaudeAPI", client =>
{
    client.BaseAddress = new Uri("https://api.anthropic.com");
    client.DefaultRequestHeaders.Add("x-api-key",
        builder.Configuration["ClaudeAPI:ApiKey"]);
    client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
    client.Timeout = TimeSpan.FromSeconds(120); // 120 ثانیه برای AI
});

// 3. ثبت سرویس‌های AI
builder.Services.AddScoped<
    IGenerateNutritionProgramAIService,
    GenerateNutritionProgramAIService>();

builder.Services.AddScoped<
    IGenerateTrainingProgramAIService,
    GenerateTrainingProgramAIService>();










//........................
var keysPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "DataProtectionKeys");
Directory.CreateDirectory(keysPath);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
    .SetApplicationName("FitCore");

//........................








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



builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(30);
});


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Admin/Auth/Login";
    options.AccessDeniedPath = "/Admin/Auth/Login";

    options.Cookie.Name = "FitCore.Auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;

    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});





//builder.Services.ConfigureApplicationCookie(options =>
//{

//    options.LoginPath = "/Admin/Auth/Login";
//    options.AccessDeniedPath = "/Admin/Auth/Login";

//    options.ExpireTimeSpan = TimeSpan.FromDays(7);
//    options.SlidingExpiration = true;

//    options.Cookie.Name = "FitCore.Auth";

//    options.Cookie.HttpOnly = true;

//    options.Cookie.IsEssential = true;

//    options.Cookie.SameSite = SameSiteMode.Lax;

//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

//});

#endregion

#region FluentValidation

//builder.Services
//    .AddValidatorsFromAssemblyContaining<RegisterUserValidator>();

builder.Services
    .AddFluentValidationAutoValidation();

#endregion

#region Dependency Injection

//builder.Services.Configure<FileStorageOptions>(
//    builder.Configuration.GetSection("FileStorage"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ElevatedRights", policy => policy.RequireRole("Admin", "SuperAdmin"));

    options.AddPolicy("FinancialManager", policy => policy.RequireRole("Admin", "SuperAdmin", "Accountant"));
});

builder.Services.AddScoped<
    FitCore.Application.Services.TrainingProgramReports.Queries.IGetTrainingProgramPdfService,
    FitCore.Application.Services.TrainingProgramReports.Queries.GetTrainingProgramPdfService>();
// ===== Common =====

builder.Services.AddScoped<IGetSetings, GetSetingService>();

builder.Services.AddScoped<ISiteSettingService, SiteSettingService>();

builder.Services.AddScoped<ISmsService, SmsService>();

builder.Services.AddMemoryCache();




builder.Services.AddSingleton<ILoginTokenStore,
    MemoryLoginTokenStore>();



// ===== Gym =====

builder.Services.AddScoped<IAddGymService, AddGymService>();

builder.Services.AddScoped<IDeleteGymService, DeleteGymService>();

builder.Services.AddScoped<IEditGymService, EditGymService>();

builder.Services.AddScoped<IGetGymByIdService, GetGymByIdService>();

builder.Services.AddScoped<IGetGymsService, GetGymsService>();

builder.Services.AddScoped<ICompleteGymInfoService, CompleteGymInfoService>();

builder.Services.AddScoped<IGetProvincesService, GetProvincesService>();

builder.Services.AddScoped<IGetCitiesService, GetCitiesService>();

builder.Services.AddHttpClient<IBaleBotService, BaleBotService>();


builder.Services.AddScoped<IAddOrUpdateMemberService, AddOrUpdateMemberService>();
builder.Services.AddScoped<IGetMemberByAppUserIdService, GetMemberByAppUserIdService>();
builder.Services.AddScoped<IGetMemberByIdService, GetMemberByIdService>();
builder.Services.AddScoped<IAddMemberBodyMeasurementService, AddMemberBodyMeasurementService>();
builder.Services.AddScoped<IEditMemberBodyMeasurementService, EditMemberBodyMeasurementService>();

builder.Services.AddScoped<IGetMemberBodyMeasurementsService, GetMemberBodyMeasurementsService>();
builder.Services.AddScoped<IRemoveBodyMeasurementService, RemoveBodyMeasurementService>();



//<------  FoodS  -------->
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IFoodFacad, FoodFacad>();
builder.Services.AddScoped<IAddFoodService, AddFoodService>();
builder.Services.AddScoped<IEditFoodService, EditFoodService>();
builder.Services.AddScoped<IDeleteFoodService, DeleteFoodService>();
builder.Services.AddScoped<IGetFoodsService, GetFoodsService>();
builder.Services.AddScoped<IGetFoodByIdService, GetFoodByIdService>();
//builder.Services.AddScoped<IFoodFacad, FoodFacad>();


//<------  ProgramBuilder  -------->
builder.Services.AddScoped<IGetProgramBuilderService, GetProgramBuilderService>();
builder.Services.AddScoped<IAddNutritionProgramDayService, AddNutritionProgramDayService>();
builder.Services.AddScoped<IAddNutritionMealService, AddNutritionMealService>();
builder.Services.AddScoped<IAddNutritionMealItemService, AddNutritionMealItemService>();
builder.Services.AddScoped<IGetBuilderLookupService, GetBuilderLookupService>();
builder.Services.AddScoped<IRemoveNutritionMealItemService, RemoveNutritionMealItemService>();
builder.Services.AddScoped<IEditNutritionMealItemService, EditNutritionMealItemService>();
builder.Services.AddScoped<IRemoveNutritionMealService, RemoveNutritionMealService>();
builder.Services.AddScoped<IEditNutritionMealService, EditNutritionMealService>();
builder.Services.AddScoped<IRemoveNutritionDayService, RemoveNutritionDayService>();
builder.Services.AddScoped<IEditNutritionDayService, EditNutritionDayService>();
builder.Services.AddScoped<IAutoGenerateNutritionDaysService, AutoGenerateNutritionDaysService>();
builder.Services.AddScoped<IGetNutritionProgramPdfService, GetNutritionProgramPdfService>();
builder.Services.AddScoped<ICopyProgramFacad, CopyProgramFacad>();
builder.Services.AddScoped<IRemoveNutritionAllDayService, RemoveNutritionAllDayService>();
builder.Services.AddScoped<IRemoveAllTrainingDaysService, RemoveAllTrainingDaysService>();


builder.Services.AddScoped<ICreateTicketService, CreateTicketService>();
builder.Services.AddScoped<IReplyTicketService, ReplyTicketService>();
builder.Services.AddScoped<ICloseTicketService, CloseTicketService>();
builder.Services.AddScoped<IGetTicketsService, GetTicketsService>();
builder.Services.AddScoped<IGetTicketDetailService, GetTicketDetailService>();
builder.Services.AddScoped<IGetOpenTicketsCountService, GetOpenTicketsCountService>();

//===== Facad =====
builder.Services.AddScoped<IMemberFacad, MemberFacad>();
builder.Services.AddScoped<INutritionProgramFacad, NutritionProgramFacad>();
//===== Facad =====

//===== Facad - Training Program Module =====
builder.Services.AddScoped<ITrainingProgramFacad, TrainingProgramFacad>();
builder.Services.AddScoped<ITrainingProgramBuilderFacad, TrainingProgramBuilderFacad>();
builder.Services.AddScoped<IExerciseFacad, ExerciseFacad>();
builder.Services.AddScoped<IGymDashboardService, GymDashboardService>();
//===== Facad - Training Program Module =====

builder.Services.AddScoped<IGetHelpContentService, GetHelpContentService>();
builder.Services.AddScoped<IHelp_Service, HelpService>();



builder.Services.AddScoped<IBaleCallbackHandler, BaleCallbackHandler>();
builder.Services.AddScoped<IBaleMessageHandler, BaleMessageHandler>();
//++++++++++++++++

builder.Services.AddScoped<IAnnouncementFacade, AnnouncementFacade>();


builder.Services.AddScoped<IAddAnnouncementService, AddAnnouncementService>();
builder.Services.AddScoped<IEditAnnouncementService, EditAnnouncementService>();
builder.Services.AddScoped<IDeleteAnnouncementService, DeleteAnnouncementService>();
builder.Services.AddScoped<IGetAnnouncementsService, GetAnnouncementsService>();
builder.Services.AddScoped<IGetAnnouncementByIdService, GetAnnouncementByIdService>();
builder.Services.AddScoped<IRegisterAnnouncementViewService, RegisterAnnouncementViewService>();
builder.Services.AddScoped<IDismissAnnouncementService, DismissAnnouncementService>();
builder.Services.AddScoped<IRegisterAnnouncementClickService, RegisterAnnouncementClickService>();

builder.Services.AddScoped<
    FitCore.Application.Services.Announcements.Queries.GetGyms.IGetGymsService,
    FitCore.Application.Services.Announcements.Queries.GetGyms.GetGymsService>();

builder.Services.AddScoped<IGetRolesService, GetRolesService>();

//++++++++++++++++


builder.Services.AddScoped<IGetMembersByIdService,
    GetMembersByIdService>();

builder.Services.AddScoped<
    FitCore.Application.FacadPatterns.IProgramRequestFacad,
    FitCore.Application.FacadPatterns.ProgramRequestFacad>();

builder.Services.AddScoped<IAddNewMemberService, AddNewMemberService>();

builder.Services.AddScoped<ISuperAdminDashboardService, SuperAdminDashboardService>();



builder.Services.AddScoped<IGetMembersService, GetMembersService>();

// ===== Auth =====


builder.Services.AddTransient<IFileCompressionService, FileCompressionService>();


builder.Services.AddScoped<IEditMemberService, EditMemberService>();

builder.Services.AddScoped<IRemoveMemberService, RemoveMemberService>();

builder.Services.AddScoped<EndPoint.Site.BaleBot.Services.IBaleMenuService, EndPoint.Site.BaleBot.Services.BaleMenuService>();

// اطمینان از ثبت SignInManager (معمولاً با AddIdentity قبلاً اضافه شده، اما اگر ارور گرفتید این خط را اضافه کنید)
builder.Services.AddScoped<SignInManager<AppUser>>();


builder.Services.AddScoped<RegisterUserService>();

builder.Services.AddScoped<RegisterManagerService>();

builder.Services.AddScoped<SendOtpService>();

builder.Services.AddScoped<VerifyOtpService>();

builder.Services.AddScoped<
    IUserClaimsPrincipalFactory<AppUser>,
    CustomClaimsPrincipalFactory>();


builder.Services.AddScoped<IGetDashboardAnnouncementsService,
                   GetDashboardAnnouncementsService>();

#endregion


builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromDays(30);
});


#region MVC

builder.Services.AddControllersWithViews();



// افزایش محدودیت حجم آپلود برای IIS و Kestrel
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 104857600; // 100 MB
});
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // 100 MB
});





var storagePath =
    FileStorageHelper.GetStoragePath(
        builder.Configuration,
        builder.Environment);

builder.Services.Configure<FileStorageOptions>(options =>
{
    options.RootPath = storagePath;
});




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion


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

QuestPDF.Settings.License = LicenseType.Community;

FontManager.RegisterFont(File.OpenRead("wwwroot/fonts/Vazir-Regular.ttf"));
FontManager.RegisterFont(File.OpenRead("wwwroot/fonts/Vazir-Bold.ttf"));
app.UseHttpsRedirection();





//*************




app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(storagePath),
    RequestPath = "/uploads"
});

//*************








app.UseRouting();


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

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

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


#endregion

app.Run();