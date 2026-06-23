using EndPoint.Site.BaleBot.Handlers;
using EndPoint.Site.BaleBot.Services;

using GymBot.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace EndPoint.Site.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class BaleWebhookController : ControllerBase
    {
        private readonly IBaleCallbackHandler _callbackHandler;
        private readonly IBaleMessageHandler _messageHandler;
        private readonly IBaleMenuService _menuService;

        public BaleWebhookController(
            IBaleCallbackHandler callbackHandler,
            IBaleMessageHandler messageHandler,
            IBaleMenuService menuService)
        {
            _callbackHandler = callbackHandler;
            _messageHandler = messageHandler;
            _menuService = menuService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BaleWebhookPayload payload)
        {
            // 1. تایید وب‌هوک
            if (!string.IsNullOrEmpty(payload.Challenge))
                return Ok(payload.Challenge);

            // 2. استخراج اطلاعات پایه از پیام بله
            long chatId = 0;
            string userText = "";
            BaleCallbackQuery callback = null;
            BaleContact contact = null;
            string userName = "کاربر";

            if (payload?.Message != null)
            {
                chatId = payload.Message.Chat.Id;
                userText = payload.Message.Text?.Trim() ?? "";
                contact = payload.Message.Contact;
                userName = payload.Message.From?.Name ?? "کاربر";
            }
            else if (payload?.CallbackQuery != null)
            {
                callback = payload.CallbackQuery;
                chatId = callback.From.Id;
                userText = callback.Data ?? "";
                userName = callback.From?.Name ?? "کاربر";
            }

            if (chatId == 0) return Ok();

            // 3. هدایت درخواست به پردازشگرهای مربوطه
            if (callback != null)
            {
                await _callbackHandler.HandleAsync(chatId, userText, callback.Id, userName);
                return Ok();
            }

            if (contact != null)
            {
                await _messageHandler.HandleContactAsync(chatId, contact.PhoneNumber);
                return Ok();
            }

            if (!string.IsNullOrEmpty(userText))
            {
                if (userText.ToLower() == "/start" || userText.ToLower() == "منوی اصلی")
                {
                    await _menuService.ShowMainMenu(chatId, userName);
                }
                else
                {
                    await _messageHandler.HandleTextAsync(chatId, userText);
                }
            }

            return Ok();
        }
    }
}