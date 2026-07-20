using FitCore.Application.Services.Auth;

using GymBot.Models;

using Microsoft.Extensions.Configuration;

using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymBot.Services;

public class BaleBotClient : IBaleBotClient
{
    private readonly HttpClient _httpClient;
    private readonly string _token;

    public BaleBotClient(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _token = configuration["BaleBot:Token"]!;
    }

    public async Task<bool> SendMessageAsync(
        long chatId,
        string text,
        InlineKeyboardMarkup? replyMarkup = null)
    {
        var url = $"https://tapi.bale.ai/bot{_token}/sendMessage";

        var payload = new BaleSendMessageRequest
        {
            ChatId = chatId,
            Text = text,
            ReplyMarkup = replyMarkup
        };

        var json = JsonSerializer.Serialize(payload);

        var response = await _httpClient.PostAsync(
            url,
            new StringContent(json, Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> SendMessageWithContactKeyboardAsync(
        long chatId,
        string text)
    {
        var url = $"https://tapi.bale.ai/bot{_token}/sendMessage";

        var keyboard = new ReplyKeyboardMarkup
        {
            Keyboard =
            [
                [
                    new KeyboardButton
                    {
                        Text="📱 ارسال شماره موبایل",
                        RequestContact=true
                    }
                ]
            ]
        };

        var payload = new
        {
            chat_id = chatId,
            text = text,
            reply_markup = keyboard
        };

        var json = JsonSerializer.Serialize(payload);

        var response = await _httpClient.PostAsync(
            url,
            new StringContent(json, Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }

    public async Task AnswerCallbackQueryAsync(
        string callbackQueryId,
        string text = "")
    {
        var url = $"https://tapi.bale.ai/bot{_token}/answerCallbackQuery";

        var payload = new
        {
            callback_query_id = callbackQueryId,
            text
        };

        var json = JsonSerializer.Serialize(payload);

        await _httpClient.PostAsync(
            url,
            new StringContent(json, Encoding.UTF8, "application/json"));
    }

    public async Task<bool> SendDocumentAsync(
        long chatId,
        string fileUrl,
        string caption = "")
    {
        var url = $"https://tapi.bale.ai/bot{_token}/sendDocument";

        var payload = new
        {
            chat_id = chatId,
            document = fileUrl,
            caption
        };

        var json = JsonSerializer.Serialize(payload);

        var response = await _httpClient.PostAsync(
            url,
            new StringContent(json, Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> SendDocumentAsync(
        long chatId,
        byte[] fileBytes,
        string fileName,
        string caption = "")
    {
        var url = $"https://tapi.bale.ai/bot{_token}/sendDocument";

        using var form = new MultipartFormDataContent();

        form.Add(new StringContent(chatId.ToString()), "chat_id");

        var file = new ByteArrayContent(fileBytes);
        file.Headers.ContentType =
            new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

        form.Add(file, "document", fileName);

        if (!string.IsNullOrWhiteSpace(caption))
            form.Add(new StringContent(caption), "caption");

        var response = await _httpClient.PostAsync(url, form);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> EditMessageTextAsync(
        long chatId,
        long messageId,
        string text,
        InlineKeyboardMarkup? replyMarkup = null)
    {
        var url = $"https://tapi.bale.ai/bot{_token}/editMessageText";

        var payload = new
        {
            chat_id = chatId,
            message_id = messageId,
            text,
            reply_markup = replyMarkup
        };

        var json = JsonSerializer.Serialize(payload);

        var response = await _httpClient.PostAsync(
            url,
            new StringContent(json, Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteMessageAsync(
        long chatId,
        long messageId)
    {
        var url = $"https://tapi.bale.ai/bot{_token}/deleteMessage";

        var payload = new
        {
            chat_id = chatId,
            message_id = messageId
        };

        var json = JsonSerializer.Serialize(payload);

        var response = await _httpClient.PostAsync(
            url,
            new StringContent(json, Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }
}