using System.Text.Json.Serialization;

namespace GymBot.Models
{
    // کلاس اصلی دریافت شده از بله
    public class BaleWebhookPayload
    {
        // برای تایید وب‌هوک
        [JsonPropertyName("challenge")]
        public string Challenge { get; set; }

        // پیام کاربر در یک شیء تودرتو قرار دارد
        [JsonPropertyName("message")]
        public BaleMessage Message { get; set; }
    }

    // کلاس جدید برای جدا کردن پیام از ریشه
    public class BaleMessage
    {
        [JsonPropertyName("message_id")]
        public long MessageId { get; set; }

        [JsonPropertyName("from")]
        public BaleUser From { get; set; }

        [JsonPropertyName("chat")]
        public BaleChat Chat { get; set; }

        [JsonPropertyName("date")]
        public long Date { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class BaleUser
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }
    }

    public class BaleChat
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }

    // مدل ارسال پیام (بدون تغییر)
    public class BaleSendMessageRequest
    {
        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}