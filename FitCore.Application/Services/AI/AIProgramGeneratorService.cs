using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Members;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FitCore.Application.Services.AI
{
    // ====================================================
    // DTO های خروجی AI
    // ====================================================

    public class AINutritionProgramDto
    {
        [JsonPropertyName("days")]
        public List<AINutritionDayDto> Days { get; set; } = new();

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("totalCalories")]
        public int TotalCalories { get; set; }

        [JsonPropertyName("totalProtein")]
        public int TotalProtein { get; set; }

        [JsonPropertyName("totalCarb")]
        public int TotalCarb { get; set; }

        [JsonPropertyName("totalFat")]
        public int TotalFat { get; set; }
    }

    public class AINutritionDayDto
    {
        [JsonPropertyName("dayNumber")]
        public int DayNumber { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("meals")]
        public List<AIMealDto> Meals { get; set; } = new();
    }

    public class AIMealDto
    {
        [JsonPropertyName("mealType")]
        public string MealType { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("foods")]
        public List<AIFoodItemDto> Foods { get; set; } = new();
    }

    public class AIFoodItemDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }
    }

    // ====================================================

    public class AITrainingProgramDto
    {
        [JsonPropertyName("days")]
        public List<AITrainingDayDto> Days { get; set; } = new();

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("weeklyPlan")]
        public string WeeklyPlan { get; set; }
    }

    public class AITrainingDayDto
    {
        [JsonPropertyName("dayNumber")]
        public int DayNumber { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("dayType")]
        public string DayType { get; set; }

        [JsonPropertyName("durationMinutes")]
        public int? DurationMinutes { get; set; }

        [JsonPropertyName("exercises")]
        public List<AIExerciseItemDto> Exercises { get; set; } = new();
    }

    public class AIExerciseItemDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("muscleGroup")]
        public string MuscleGroup { get; set; }

        [JsonPropertyName("sets")]
        public int? Sets { get; set; }

        [JsonPropertyName("reps")]
        public string Reps { get; set; }

        [JsonPropertyName("restSeconds")]
        public int? RestSeconds { get; set; }

        [JsonPropertyName("coachNote")]
        public string CoachNote { get; set; }
    }

    // ====================================================
    // پروفایل عضو برای ارسال به AI
    // ====================================================

    public class MemberProfileForAIDto
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public decimal? BodyFatPercentage { get; set; }
        public string ActivityLevel { get; set; }
        public string ExperienceLevel { get; set; }
        public string FoodAllergies { get; set; }
        public string MedicalConditions { get; set; }
        public string Injuries { get; set; }
        public string Goal { get; set; }
        public int DaysCount { get; set; }
        public string MemberNote { get; set; }
    }

    // ====================================================
    // سرویس اصلی — تولید برنامه غذایی با AI
    // ====================================================

    public interface IGenerateNutritionProgramAIService
    {
        Task<ResultDto<AINutritionProgramDto>> Execute(
            long memberId, string goal, int daysCount, string memberNote = "");
    }

    public class GenerateNutritionProgramAIService : IGenerateNutritionProgramAIService
    {
        private readonly IDataBaseContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public GenerateNutritionProgramAIService(
            IDataBaseContext context,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResultDto<AINutritionProgramDto>> Execute(
            long memberId, string goal, int daysCount, string memberNote = "")
        {
            var profile = await BuildMemberProfileAsync(memberId, goal, daysCount, memberNote);
            if (profile == null)
                return ResultDto<AINutritionProgramDto>.Failure("اطلاعات عضو یافت نشد.");

            var prompt = BuildNutritionPrompt(profile);
            var json   = await CallClaudeAPIAsync(prompt);

            if (string.IsNullOrWhiteSpace(json))
                return ResultDto<AINutritionProgramDto>.Failure("خطا در دریافت پاسخ از هوش مصنوعی.");

            try
            {
                var result = JsonSerializer.Deserialize<AINutritionProgramDto>(
                    json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return ResultDto<AINutritionProgramDto>.Success(
                    result, "برنامه غذایی با موفقیت توسط هوش مصنوعی تولید شد.");
            }
            catch
            {
                return ResultDto<AINutritionProgramDto>.Failure(
                    "خطا در پردازش پاسخ هوش مصنوعی. لطفاً دوباره امتحان کنید.");
            }
        }

        private string BuildNutritionPrompt(MemberProfileForAIDto p)
        {
            var sb = new StringBuilder();

            sb.AppendLine("You are an expert Iranian sports nutritionist. Generate a personalized nutrition program in Persian.");
            sb.AppendLine("");
            sb.AppendLine("MEMBER PROFILE:");
            sb.AppendLine($"- Name: {p.FullName}");
            sb.AppendLine($"- Gender: {p.Gender}");
            if (p.Height.HasValue) sb.AppendLine($"- Height: {p.Height} cm");
            if (p.Weight.HasValue) sb.AppendLine($"- Weight: {p.Weight} kg");
            if (p.BodyFatPercentage.HasValue) sb.AppendLine($"- Body Fat: {p.BodyFatPercentage}%");
            sb.AppendLine($"- Activity Level: {p.ActivityLevel ?? "متوسط"}");
            sb.AppendLine($"- Experience: {p.ExperienceLevel ?? "مبتدی"}");
            sb.AppendLine($"- Goal: {p.Goal}");
            if (!string.IsNullOrWhiteSpace(p.FoodAllergies))
                sb.AppendLine($"- Food Allergies/Restrictions: {p.FoodAllergies}");
            if (!string.IsNullOrWhiteSpace(p.MedicalConditions))
                sb.AppendLine($"- Medical Conditions: {p.MedicalConditions}");
            if (!string.IsNullOrWhiteSpace(p.MemberNote))
                sb.AppendLine($"- Member's Notes: {p.MemberNote}");
            sb.AppendLine($"- Program Duration: {p.DaysCount} days");
            sb.AppendLine("");
            sb.AppendLine("REQUIREMENTS:");
            sb.AppendLine("1. Create a realistic, practical nutrition plan");
            sb.AppendLine("2. Use common Iranian foods (برنج، مرغ، تخم‌مرغ، ماهی، سبزیجات، حبوبات، etc.)");
            sb.AppendLine("3. Include 5-6 meals per day (صبحانه، میان‌وعده صبح، ناهار، میان‌وعده عصر، شام، قبل از خواب)");
            sb.AppendLine("4. Respect all food allergies and medical conditions strictly");
            sb.AppendLine("5. All food names and instructions must be in Persian");
            sb.AppendLine("6. Vary meals across different days for nutritional diversity");
            sb.AppendLine("");
            sb.AppendLine("RESPOND ONLY WITH VALID JSON in this exact format, no other text:");
            sb.AppendLine(@"{
  ""summary"": ""خلاصه برنامه و توضیحات مربی"",
  ""totalCalories"": 2000,
  ""totalProtein"": 150,
  ""totalCarb"": 220,
  ""totalFat"": 65,
  ""days"": [
    {
      ""dayNumber"": 1,
      ""title"": ""روز اول"",
      ""meals"": [
        {
          ""mealType"": ""صبحانه"",
          ""title"": ""صبحانه پروتئینه"",
          ""foods"": [
            { ""name"": ""تخم‌مرغ آب‌پز"", ""amount"": 3, ""unit"": ""عدد"", ""note"": """" },
            { ""name"": ""نان سنگک"", ""amount"": 1, ""unit"": ""برش"", ""note"": """" }
          ]
        }
      ]
    }
  ]
}");

            return sb.ToString();
        }

        private async Task<MemberProfileForAIDto> BuildMemberProfileAsync(
            long memberId, string goal, int daysCount, string memberNote)
        {
            var member = await _context.Members
                .Include(m => m.AppUser)
                .Include(m => m.ActivityLevel)
                .Include(m => m.ExperienceLevel)
                .Include(m => m.memberBodyMeasurements)
                .FirstOrDefaultAsync(m => m.Id == memberId);

            if (member == null) return null;

            var lastMeasurement = member.memberBodyMeasurements?
                .Where(x => !x.IsRemoved)
                .OrderByDescending(x => x.InsertTime)
                .FirstOrDefault();

            return new MemberProfileForAIDto
            {
                FullName           = member.AppUser?.FullName,
                Gender             = member.Gender == Gender.Male ? "مرد" : "زن",
                Height             = member.Height,
                Weight             = lastMeasurement?.Weight,
                BodyFatPercentage  = lastMeasurement?.BodyFatPercentage,
                ActivityLevel      = member.ActivityLevel?.Name,
                ExperienceLevel    = member.ExperienceLevel?.Name,
                FoodAllergies      = member.FoodAllergies,
                MedicalConditions  = member.MedicalConditions,
                Injuries           = member.Injuries,
                Goal               = goal,
                DaysCount          = daysCount,
                MemberNote         = memberNote
            };
        }

        private async Task<string> CallClaudeAPIAsync(string prompt)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ClaudeAPI");

                var requestBody = new
                {
                    model      = "claude-sonnet-4-6",
                    max_tokens = 4000,
                    messages   = new[]
                    {
                        new { role = "user", content = prompt }
                    }
                };

                var json    = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/v1/messages", content);

                if (!response.IsSuccessStatusCode)
                    return null;

                var responseJson = await response.Content.ReadAsStringAsync();
                using var doc    = JsonDocument.Parse(responseJson);

                var text = doc.RootElement
                    .GetProperty("content")[0]
                    .GetProperty("text")
                    .GetString();

                // پاکسازی markdown code blocks اگر AI اضافه کرده باشد
                if (text != null)
                {
                    text = text.Trim();
                    if (text.StartsWith("```json")) text = text[7..];
                    if (text.StartsWith("```"))     text = text[3..];
                    if (text.EndsWith("```"))        text = text[..^3];
                    text = text.Trim();
                }

                return text;
            }
            catch
            {
                return null;
            }
        }
    }

    // ====================================================
    // سرویس — تولید برنامه تمرینی با AI
    // ====================================================

    public interface IGenerateTrainingProgramAIService
    {
        Task<ResultDto<AITrainingProgramDto>> Execute(
            long memberId, string goal, string programType,
            int daysPerWeek, int totalDays, string memberNote = "");
    }

    public class GenerateTrainingProgramAIService : IGenerateTrainingProgramAIService
    {
        private readonly IDataBaseContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public GenerateTrainingProgramAIService(
            IDataBaseContext context,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResultDto<AITrainingProgramDto>> Execute(
            long memberId, string goal, string programType,
            int daysPerWeek, int totalDays, string memberNote = "")
        {
            var profile = await BuildMemberProfileAsync(memberId, goal, totalDays, memberNote);
            if (profile == null)
                return ResultDto<AITrainingProgramDto>.Failure("اطلاعات عضو یافت نشد.");

            var prompt = BuildTrainingPrompt(profile, programType, daysPerWeek);
            var json   = await CallClaudeAPIAsync(prompt);

            if (string.IsNullOrWhiteSpace(json))
                return ResultDto<AITrainingProgramDto>.Failure("خطا در دریافت پاسخ از هوش مصنوعی.");

            try
            {
                var result = JsonSerializer.Deserialize<AITrainingProgramDto>(
                    json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return ResultDto<AITrainingProgramDto>.Success(
                    result, "برنامه تمرینی با موفقیت توسط هوش مصنوعی تولید شد.");
            }
            catch
            {
                return ResultDto<AITrainingProgramDto>.Failure(
                    "خطا در پردازش پاسخ هوش مصنوعی. لطفاً دوباره امتحان کنید.");
            }
        }

        private string BuildTrainingPrompt(
            MemberProfileForAIDto p, string programType, int daysPerWeek)
        {
            var sb = new StringBuilder();

            sb.AppendLine("You are an expert Iranian strength and conditioning coach. Generate a personalized training program in Persian.");
            sb.AppendLine("");
            sb.AppendLine("MEMBER PROFILE:");
            sb.AppendLine($"- Name: {p.FullName}");
            sb.AppendLine($"- Gender: {p.Gender}");
            if (p.Height.HasValue) sb.AppendLine($"- Height: {p.Height} cm");
            if (p.Weight.HasValue) sb.AppendLine($"- Weight: {p.Weight} kg");
            if (p.BodyFatPercentage.HasValue) sb.AppendLine($"- Body Fat: {p.BodyFatPercentage}%");
            sb.AppendLine($"- Activity Level: {p.ActivityLevel ?? "متوسط"}");
            sb.AppendLine($"- Experience: {p.ExperienceLevel ?? "مبتدی"}");
            sb.AppendLine($"- Goal: {p.Goal}");
            sb.AppendLine($"- Program Type: {programType}");
            sb.AppendLine($"- Training Days Per Week: {daysPerWeek}");
            sb.AppendLine($"- Total Days: {p.DaysCount}");
            if (!string.IsNullOrWhiteSpace(p.Injuries))
                sb.AppendLine($"- Injuries/Limitations: {p.Injuries}");
            if (!string.IsNullOrWhiteSpace(p.MedicalConditions))
                sb.AppendLine($"- Medical Conditions: {p.MedicalConditions}");
            if (!string.IsNullOrWhiteSpace(p.MemberNote))
                sb.AppendLine($"- Notes: {p.MemberNote}");
            sb.AppendLine("");
            sb.AppendLine("REQUIREMENTS:");
            sb.AppendLine("1. Design a structured, progressive training program");
            sb.AppendLine("2. Use gym equipment exercises (machines, barbells, dumbbells, cables)");
            sb.AppendLine("3. Include proper warm-up considerations");
            sb.AppendLine("4. Avoid exercises that conflict with injuries/medical conditions");
            sb.AppendLine("5. All exercise names and instructions MUST be in Persian");
            sb.AppendLine("6. Provide realistic sets, reps, and rest periods");
            sb.AppendLine("7. Include coach notes for technique cues where important");
            sb.AppendLine("");
            sb.AppendLine("RESPOND ONLY WITH VALID JSON in this exact format, no other text:");
            sb.AppendLine(@"{
  ""summary"": ""خلاصه برنامه تمرینی"",
  ""weeklyPlan"": ""روز ۱: سینه و سه‌سر | روز ۲: پشت و دوسر | ..."",
  ""days"": [
    {
      ""dayNumber"": 1,
      ""title"": ""سینه و سه‌سر"",
      ""dayType"": ""Push"",
      ""durationMinutes"": 75,
      ""exercises"": [
        {
          ""name"": ""پرس سینه هالتر"",
          ""muscleGroup"": ""سینه"",
          ""sets"": 4,
          ""reps"": ""8-12"",
          ""restSeconds"": 90,
          ""coachNote"": ""پاها روی زمین، کمر کمی قوس طبیعی""
        }
      ]
    }
  ]
}");

            return sb.ToString();
        }

        private async Task<MemberProfileForAIDto> BuildMemberProfileAsync(
            long memberId, string goal, int totalDays, string memberNote)
        {
            var member = await _context.Members
                .Include(m => m.AppUser)
                .Include(m => m.ActivityLevel)
                .Include(m => m.ExperienceLevel)
                .Include(m => m.memberBodyMeasurements)
                .FirstOrDefaultAsync(m => m.Id == memberId);

            if (member == null) return null;

            var lastMeasurement = member.memberBodyMeasurements?
                .Where(x => !x.IsRemoved)
                .OrderByDescending(x => x.InsertTime)
                .FirstOrDefault();

            return new MemberProfileForAIDto
            {
                FullName          = member.AppUser?.FullName,
                Gender            = member.Gender == Gender.Male ? "مرد" : "زن",
                Height            = member.Height,
                Weight            = lastMeasurement?.Weight,
                BodyFatPercentage = lastMeasurement?.BodyFatPercentage,
                ActivityLevel     = member.ActivityLevel?.Name,
                ExperienceLevel   = member.ExperienceLevel?.Name,
                Injuries          = member.Injuries,
                MedicalConditions = member.MedicalConditions,
                Goal              = goal,
                DaysCount         = totalDays,
                MemberNote        = memberNote
            };
        }

        private async Task<string> CallClaudeAPIAsync(string prompt)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ClaudeAPI");

                var requestBody = new
                {
                    model      = "claude-sonnet-4-6",
                    max_tokens = 4000,
                    messages   = new[]
                    {
                        new { role = "user", content = prompt }
                    }
                };

                var json    = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/v1/messages", content);

                if (!response.IsSuccessStatusCode)
                    return null;

                var responseJson = await response.Content.ReadAsStringAsync();
                using var doc    = JsonDocument.Parse(responseJson);

                var text = doc.RootElement
                    .GetProperty("content")[0]
                    .GetProperty("text")
                    .GetString();

                if (text != null)
                {
                    text = text.Trim();
                    if (text.StartsWith("```json")) text = text[7..];
                    if (text.StartsWith("```"))     text = text[3..];
                    if (text.EndsWith("```"))        text = text[..^3];
                    text = text.Trim();
                }

                return text;
            }
            catch
            {
                return null;
            }
        }
    }
}
