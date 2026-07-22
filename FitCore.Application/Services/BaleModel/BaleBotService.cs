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

        public Task AnswerCallbackQueryAsync(string callbackQueryId, string text = "", bool showAlert = false)
        {
            return _client.AnswerCallbackQueryAsync(callbackQueryId, text, showAlert);
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

        public Task<bool> EditMessageTextAsync(long chatId, long messageId, string text, InlineKeyboardMarkup? replyMarkup = null)
        {
            return _client.EditMessageTextAsync(chatId, messageId, text, replyMarkup);
        }

        public Task<bool> DeleteMessageAsync(long chatId, long messageId)
        {
            return _client.DeleteMessageAsync(chatId, messageId);
        }


        public Task SendChatActionAsync(long chatId, string action = "typing")
        {
            return _client.SendChatActionAsync(chatId, action);
        }
    }



}

