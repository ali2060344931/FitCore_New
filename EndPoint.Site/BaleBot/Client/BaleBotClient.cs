using GymBot.Models;

using System;
using System.Threading.Tasks;

namespace EndPoint.Site.BaleBot.Client
{
    /// <summary>
    /// ارتباط مستقیم با API بله
    /// </summary>
    public class BaleBotClient : IBaleBotClient
    {
        public BaleBotClient()
        {

        }

        public Task SendMessageAsync(
            long chatId,
            string text,
            InlineKeyboardMarkup? keyboard = null)
        {
            throw new NotImplementedException();
        }

        public Task EditMessageTextAsync(
            long chatId,
            long messageId,
            string text,
            InlineKeyboardMarkup? keyboard = null)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMessageAsync(
            long chatId,
            long messageId)
        {
            throw new NotImplementedException();
        }

        public Task AnswerCallbackQueryAsync(
            string callbackQueryId,
            string? text = null,
            bool showAlert = false)
        {
            throw new NotImplementedException();
        }
    }
}