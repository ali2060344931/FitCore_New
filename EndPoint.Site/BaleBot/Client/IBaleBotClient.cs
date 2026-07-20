using GymBot.Models;

using System.Threading.Tasks;

namespace EndPoint.Site.BaleBot.Client
{
    /// <summary>
    /// ارتباط مستقیم با API ربات بله
    /// </summary>
    public interface IBaleBotClient
    {
        Task SendMessageAsync(
            long chatId,
            string text,
            InlineKeyboardMarkup? keyboard = null);

        Task EditMessageTextAsync(
            long chatId,
            long messageId,
            string text,
            InlineKeyboardMarkup? keyboard = null);

        Task DeleteMessageAsync(
            long chatId,
            long messageId);

        Task AnswerCallbackQueryAsync(
            string callbackQueryId,
            string? text = null,
            bool showAlert = false);
    }
}