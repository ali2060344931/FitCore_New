using System.Collections.Generic;
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


        // بسیار مهم: وقتی کاربر روی دکمه شیشه‌ای کلیک می‌کند، این پر می‌شود
        [JsonPropertyName("callback_query")]
        public BaleCallbackQuery CallbackQuery { get; set; }
    }


    // کلاس جدید برای دریافت اطلاعات کلیک
    public class BaleCallbackQuery
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("from")]
        public BaleUser From { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("message")]
        public BaleCallbackMessage Message { get; set; }
    }



    public class BaleCallbackMessage
    {
        [JsonPropertyName("message_id")]
        public long MessageId { get; set; }

        [JsonPropertyName("chat")]
        public BaleChat Chat { get; set; }
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


        // اضافه شدن این خط برای دریافت شماره تماس
        [JsonPropertyName("contact")]
        public BaleContact Contact { get; set; }
    }


    // ساخت کلاس مربوط به مخاطب
    public class BaleContact
    {
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }
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

        // اضافه شدن دکمه‌ها (اختیاری است، اگر نال باشد دکمه‌ای نمی‌آید)
        [JsonPropertyName("reply_markup")]
        public InlineKeyboardMarkup ReplyMarkup { get; set; }
    }

    // مدل‌های مربوط به دکمه‌های شیشه‌ای
    public class InlineKeyboardButton
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("callback_data")]
        public string CallbackData { get; set; }
    }

    public class InlineKeyboardMarkup
    {
        [JsonPropertyName("inline_keyboard")]
        public List<List<InlineKeyboardButton>> InlineKeyboard { get; set; }
    }



    public class ReplyKeyboardMarkup
    {
        [JsonPropertyName("keyboard")]
        public List<List<KeyboardButton>> Keyboard { get; set; }

        [JsonPropertyName("resize_keyboard")]
        public bool ResizeKeyboard { get; set; } = true;

        [JsonPropertyName("one_time_keyboard")]
        public bool OneTimeKeyboard { get; set; } = false;
    }

    public class KeyboardButton
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("request_contact")]
        public bool RequestContact { get; set; } = false;

        [JsonPropertyName("request_location")]
        public bool RequestLocation { get; set; } = false;
    }
    public class ReplyKeyboardRemove
    {
        [JsonPropertyName("remove_keyboard")]
        public bool RemoveKeyboard { get; set; } = true;

        [JsonPropertyName("selective")]
        public bool Selective { get; set; } = false;
    }
}

