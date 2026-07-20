using GymBot.Models;

using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public interface IBaleBotService
    {
        Task<bool> SendMessageAsync(long chatId, string text, InlineKeyboardMarkup replyMarkup = null);
        Task AnswerCallbackQueryAsync(string callbackQueryId, string text = "");
        Task<bool> SendMessageWithContactKeyboardAsync(long chatId, string text);
        Task<bool> SendDocumentAsync(long chatId, string fileUrl, string caption = "");
        Task<bool> SendDocumentAsync(long chatId, byte[] fileBytes, string fileName, string caption = "");
    }
}
