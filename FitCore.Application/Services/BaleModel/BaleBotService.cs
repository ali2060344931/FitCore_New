using FitCore.Application.Services.Auth;

using GymBot.Models;

using System.Threading.Tasks;

namespace GymBot.Services
{
    public class BaleBotService : IBaleBotService
    {
        private readonly IBaleBotClient _client;

        public BaleBotService(IBaleBotClient client)
        {
            _client = client;
        }

        public Task<bool> SendMessageAsync(
            long chatId,
            string text,
            InlineKeyboardMarkup replyMarkup = null)
        {
            return _client.SendMessageAsync(chatId, text, replyMarkup);
        }

        public Task AnswerCallbackQueryAsync(
            string callbackQueryId,
            string text = "")
        {
            return _client.AnswerCallbackQueryAsync(callbackQueryId, text);
        }

        public Task<bool> SendMessageWithContactKeyboardAsync(
            long chatId,
            string text)
        {
            return _client.SendMessageWithContactKeyboardAsync(chatId, text);
        }

        public Task<bool> SendDocumentAsync(
            long chatId,
            string fileUrl,
            string caption = "")
        {
            return _client.SendDocumentAsync(chatId, fileUrl, caption);
        }

        public Task<bool> SendDocumentAsync(
            long chatId,
            byte[] fileBytes,
            string fileName,
            string caption = "")
        {
            return _client.SendDocumentAsync(chatId, fileBytes, fileName, caption);
        }
    }
}




//using FitCore.Application.Services.Auth;



//using GymBot.Models;

//using Microsoft.Extensions.Configuration;

//using System.Collections.Generic;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace GymBot.Services
//{
//    public class BaleBotService : IBaleBotService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly string _token;

//        // تغییر اینجا: دریافت مستقیم HttpClient به جای IHttpClientFactory
//        public BaleBotService(HttpClient httpClient, IConfiguration configuration)
//        {
//            _httpClient = httpClient;
//            _token = configuration["BaleBot:Token"];
//        }

//        public async Task<bool> SendMessageAsync(long chatId, string text, InlineKeyboardMarkup replyMarkup = null)
//        {
//            var url = $"https://tapi.bale.ai/bot{_token}/sendMessage";

//            // ساخت آبجکت کامل شامل متن و دکمه‌ها
//            var payload = new BaleSendMessageRequest
//            {
//                ChatId = chatId,
//                Text = text,
//                ReplyMarkup = replyMarkup // این خط باعث می‌شود دکمه‌ها به JSON اضافه شوند
//            };

//            // سریالایز کردن کل payload
//            var jsonPayload = JsonSerializer.Serialize(payload);
//            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

//            var response = await _httpClient.PostAsync(url, content);
//            return response.IsSuccessStatusCode;
//        }


//        public async Task AnswerCallbackQueryAsync(string callbackQueryId, string text = "")
//        {
//            var url = $"https://tapi.bale.ai/bot{_token}/answerCallbackQuery";
//            var payload = new
//            {
//                callback_query_id = callbackQueryId,
//                text = text // متنی که به صورت Toast به کاربر نشان داده می‌شود
//            };
//            var jsonPayload = JsonSerializer.Serialize(payload);
//            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
//            await _httpClient.PostAsync(url, content);
//        }




//        public async Task<bool> SendMessageWithContactKeyboardAsync(long chatId, string text)
//        {
//            var url = $"https://tapi.bale.ai/bot{_token}/sendMessage";

//            var keyboard = new ReplyKeyboardMarkup
//            {
//                Keyboard = new List<List<KeyboardButton>>
//        {
//            new List<KeyboardButton>
//            {
//                new KeyboardButton { Text = "📱 ارسال شماره موبایل", RequestContact = true }
//            }
//        }
//            };

//            var payload = new
//            {
//                chat_id = chatId,
//                text = text,
//                reply_markup = keyboard
//            };

//            var jsonPayload = JsonSerializer.Serialize(payload);
//            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
//            var response = await _httpClient.PostAsync(url, content);
//            return response.IsSuccessStatusCode;
//        }


//        public async Task<bool> SendDocumentAsync(long chatId, string fileUrl, string caption = "")
//        {
//            var url = $"https://tapi.bale.ai/bot{_token}/sendDocument";
//            var payload = new
//            {
//                chat_id = chatId,
//                document = fileUrl, // آدرس کامل فایل PDF
//                caption = caption    // متنی که زیر فایل نوشته می‌شود
//            };
//            var jsonPayload = JsonSerializer.Serialize(payload);
//            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
//            var response = await _httpClient.PostAsync(url, content);
//            return response.IsSuccessStatusCode;
//        }

//        public async Task<bool> SendDocumentAsync(long chatId, byte[] fileBytes, string fileName, string caption = "")
//        {
//            var url = $"https://tapi.bale.ai/bot{_token}/sendDocument";

//            using var content = new MultipartFormDataContent();

//            // اضافه کردن شناسه چت
//            content.Add(new StringContent(chatId.ToString()), "chat_id");

//            // اضافه کردن فایل PDF به عنوان بایت
//            var byteContent = new ByteArrayContent(fileBytes);
//            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
//            content.Add(byteContent, "document", fileName);

//            // اضافه کردن کپشن (زیر فایل)
//            if (!string.IsNullOrEmpty(caption))
//            {
//                content.Add(new StringContent(caption), "caption");
//            }

//            var response = await _httpClient.PostAsync(url, content);
//            return response.IsSuccessStatusCode;
//        }


//    }
//}