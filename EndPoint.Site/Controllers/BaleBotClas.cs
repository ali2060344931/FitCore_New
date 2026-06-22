using FitCore.Application.Services.Auth;

using GymBot.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace EndPoint.Site.Controllers
{
    public class BaleBotClas
    {


        public async Task SendSurveyToBale(long chatId, IBaleBotService _baleBotService)
        {
            string TextMessge = "نظر سنجی\n" +
                                "همکارم محترم، با سلام\n" +
                                "لطفا نظر خود را نسبت به عملکرد *شرکت بیمه پارسیان* در خصوص خدمات بیمه تکمیلی در سال 1404 را اعلام بفرمائید. نظر سنجی شما قطعا در بهبود خدمات بیمه تکمیلی در دوره جدید موثر خواهد بود.\n" +
                                "با تشکر- امور اداری\n";

            // ساخت دکمه‌ها
            var btn1 = new InlineKeyboardButton { Text = "ضعیف", CallbackData = "SRV_Survey1" };
            var btn2 = new InlineKeyboardButton { Text = "متوسط", CallbackData = "SRV_Survey2" };
            var btn3 = new InlineKeyboardButton { Text = "خوب", CallbackData = "SRV_Survey3" };
            var btn4 = new InlineKeyboardButton { Text = "عالی", CallbackData = "SRV_Survey4" };

            // قرار دادن دکمه‌ها در یک ردیف
            var row = new List<InlineKeyboardButton> { btn1, btn2, btn3, btn4 };

            // قرار دادن ردیف در کیبورد
            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>> { row }
            };

            // ساخت پayload ارسال
            var request = new BaleSendMessageRequest
            {
                ChatId = chatId,
                Text = TextMessge,
                ReplyMarkup = keyboard // پاس دادن کیبورد به پیام
            };

            // ارسال به بله (از طریق سرویس)
            await _baleBotService.SendMessageAsync(request.ChatId, request.Text, request.ReplyMarkup);

            // نکته: چون در SendMessageAsync فعلی ما فقط text و chatId را می‌فرستیم، 
            // باید متد SendMessageAsync در سرویس را کمی تغییر دهید که در پایین توضیح داده‌ام.
        }

    }
}
