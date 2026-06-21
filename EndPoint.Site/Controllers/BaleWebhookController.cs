using FitCore.Application.Services.Auth;

using GymBot.Models;
using GymBot.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace EndPoint.Site.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] // بسیار مهم: اجازه دسترسی بدون لاگین به وب‌هوک بله
    public class BaleWebhookController : ControllerBase
    {
        private readonly IBaleBotService _baleBotService;
        private readonly ILogger<BaleWebhookController> _logger;

        public BaleWebhookController(IBaleBotService baleBotService, ILogger<BaleWebhookController> logger)
        {
            _baleBotService = baleBotService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BaleWebhookPayload payload)
        {
            // ۱. تایید وب‌هوک
            if (!string.IsNullOrEmpty(payload.Challenge))
            {
                return Ok(payload.Challenge);
            }

            // ۲. پردازش پیام (تغییر از payload.Chat به payload.Message?.Chat)
            if (payload?.Message?.Chat != null && !string.IsNullOrEmpty(payload.Message.Text))
            {
                var chatId = payload.Message.Chat.Id;
                var userText = payload.Message.Text.Trim().ToLower();
                var userName = payload.Message.From?.Name ?? "کاربر";

                _logger.LogInformation("Bale Message from {User} ({ChatId}): {Text}", userName, chatId, userText);

                string responseText;

                switch (userText)
                {
                    case "/start":
                        responseText = $"سلام {userName} عزیز به ربات باشگاه بدنسازی ما خوش آمدید.\nلطفاً شماره موبایل خود را وارد کنید تا وضعیت عضویت شما بررسی شود:";
                        break;

                    case "09123456789":
                        responseText = "عضویت شما تا ۱۴۰۳/۰۹/۰۹ معتبر است. از تمرینات خود لذت ببرید!";
                        break;

                    case "اطلاعات کلاس‌ها":
                        responseText = "زمانبندی کلاس‌ها:\n- ایروبیک: شنبه و سه‌شنبه ۱۸:۰۰\n- بدنسازی: روزهای زوج ۱۹:۰۰\n- یوگا: پنجشنبه ۱۰:۰۰";
                        break;

                    default:
                        responseText = "متوجه نشدم. لطفاً از منوهای زیر یا دستورات معتبر استفاده کنید.";
                        break;
                }

                // ۳. ارسال پاسخ به کاربر (در داخل Try-Catch تا اگر خطایی داد کرش نکند)
                try
                {
                    await _baleBotService.SendMessageAsync(chatId, responseText);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending message to Bale for ChatId: {ChatId}", chatId);
                }
            }

            return Ok();
        }
    }
}