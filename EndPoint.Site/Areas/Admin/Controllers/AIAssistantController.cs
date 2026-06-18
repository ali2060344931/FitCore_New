using FitCore.Application.Contexts;
using FitCore.Application.Services.AI;
using FitCore.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AIAssistantController : Controller
    {
        private readonly IGenerateNutritionProgramAIService _nutritionAI;
        private readonly IGenerateTrainingProgramAIService  _trainingAI;
        private readonly IDataBaseContext _context;

        public AIAssistantController(
            IGenerateNutritionProgramAIService nutritionAI,
            IGenerateTrainingProgramAIService  trainingAI,
            IDataBaseContext context)
        {
            _nutritionAI = nutritionAI;
            _trainingAI  = trainingAI;
            _context     = context;
        }

        // ============================================================
        // تولید پیش‌نویس برنامه غذایی با AI
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateNutritionProgram(
            long nutritionProgramId,
            string goal,
            int daysCount = 7,
            string memberNote = "")
        {
            // پیدا کردن MemberId از برنامه
            var program = await _context.NutritionPrograms
                .FirstOrDefaultAsync(p => p.Id == nutritionProgramId);

            if (program == null)
                return Json(new { isSuccess = false, message = "برنامه غذایی یافت نشد." });

            var result = await _nutritionAI.Execute(
                program.MemberId, goal, daysCount, memberNote);

            if (!result.IsSuccess)
                return Json(new { isSuccess = false, message = result.Message });

            return Json(new
            {
                isSuccess = true,
                message   = result.Message,
                data      = result.Data
            });
        }

        // ============================================================
        // تولید پیش‌نویس برنامه تمرینی با AI
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateTrainingProgram(
            long trainingProgramId,
            string goal,
            string programType  = "بدنسازی (هایپرتروفی)",
            int daysPerWeek     = 4,
            int totalDays       = 28,
            string memberNote   = "")
        {
            var program = await _context.TrainingPrograms
                .FirstOrDefaultAsync(p => p.Id == trainingProgramId);

            if (program == null)
                return Json(new { isSuccess = false, message = "برنامه تمرینی یافت نشد." });

            var result = await _trainingAI.Execute(
                program.MemberId, goal, programType, daysPerWeek, totalDays, memberNote);

            if (!result.IsSuccess)
                return Json(new { isSuccess = false, message = result.Message });

            return Json(new
            {
                isSuccess = true,
                message   = result.Message,
                data      = result.Data
            });
        }

        // ============================================================
        // چت‌بات مربی مجازی — پاسخ به سوالات عضو
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AskCoach(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return Json(new { isSuccess = false, message = "سوال خالی است." });

            // گرفتن اطلاعات عضو برای Context
            string memberContext = "";
            if (User.IsInRole("Member"))
            {
                var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var member = await _context.Members
                    .Include(m => m.AppUser)
                    .Include(m => m.ActivityLevel)
                    .Include(m => m.ExperienceLevel)
                    .Include(m => m.memberBodyMeasurements)
                    .FirstOrDefaultAsync(m => m.AppUserId == userId);

                if (member != null)
                {
                    var lastM = member.memberBodyMeasurements?
                        .Where(x => !x.IsRemoved)
                        .OrderByDescending(x => x.InsertTime)
                        .FirstOrDefault();

                    memberContext = $"اطلاعات عضو: " +
                        $"جنسیت={( member.Gender == FitCore.Domain.Entities.Members.Gender.Male ? "مرد" : "زن" )}, " +
                        $"قد={member.Height?.ToString() ?? "نامشخص"} سانتی‌متر, " +
                        $"وزن={lastM?.Weight?.ToString() ?? "نامشخص"} کیلوگرم, " +
                        $"سطح فعالیت={member.ActivityLevel?.Name ?? "نامشخص"}, " +
                        $"آلرژی غذایی={member.FoodAllergies ?? "ندارد"}, " +
                        $"بیماری‌ها={member.MedicalConditions ?? "ندارد"}";
                }
            }

            var fullPrompt = string.IsNullOrWhiteSpace(memberContext)
                ? $"شما یک مربی ورزشی و متخصص تغذیه ایرانی هستید. به سوال زیر به فارسی، کوتاه و کاربردی پاسخ دهید:\n\n{question}"
                : $"شما یک مربی ورزشی و متخصص تغذیه ایرانی هستید. {memberContext}\n\nبا توجه به اطلاعات این عضو، به سوال زیر به فارسی، کوتاه و کاربردی پاسخ دهید:\n\n{question}";

            var httpClient = HttpContext.RequestServices
                .GetService(typeof(System.Net.Http.IHttpClientFactory)) as System.Net.Http.IHttpClientFactory;

            var client = httpClient!.CreateClient("ClaudeAPI");

            var requestBody = new
            {
                model      = "claude-sonnet-4-6",
                max_tokens = 800,
                messages   = new[] { new { role = "user", content = fullPrompt } }
            };

            var json    = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new System.Net.Http.StringContent(
                json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/v1/messages", content);
            if (!response.IsSuccessStatusCode)
                return Json(new { isSuccess = false, message = "خطا در ارتباط با هوش مصنوعی." });

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc    = System.Text.Json.JsonDocument.Parse(responseJson);
            var answer       = doc.RootElement
                .GetProperty("content")[0]
                .GetProperty("text")
                .GetString();

            return Json(new { isSuccess = true, answer });
        }
    }
}
