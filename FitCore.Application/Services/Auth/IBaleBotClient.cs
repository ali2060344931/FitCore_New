using GymBot.Models;

using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth;

public interface IBaleBotClient
{
    Task<bool> SendMessageAsync(
        long chatId,
        string text,
        InlineKeyboardMarkup? replyMarkup = null);

    Task<bool> SendMessageWithContactKeyboardAsync(
        long chatId,
        string text);

    Task AnswerCallbackQueryAsync(
        string callbackQueryId,
        string text = "");

    Task<bool> SendDocumentAsync(
        long chatId,
        string fileUrl,
        string caption = "");

    Task<bool> SendDocumentAsync(
        long chatId,
        byte[] fileBytes,
        string fileName,
        string caption = "");



    // برای آینده

    Task<bool> EditMessageTextAsync(
        long chatId,
        long messageId,
        string text,
        InlineKeyboardMarkup? replyMarkup = null);

    Task<bool> DeleteMessageAsync(
        long chatId,
        long messageId);
}